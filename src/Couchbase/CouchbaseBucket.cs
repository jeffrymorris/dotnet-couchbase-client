using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Core.Configuration.Server;
using Couchbase.Core.IO;
using Couchbase.Core.IO.Authentication;
using Couchbase.Core.IO.Connections;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Core.IO.Operations.Legacy.Authentication;
using Couchbase.Core.IO.Operations.Legacy.Collections;
using Couchbase.Core.IO.Transcoders;
using Couchbase.Core.Sharding;
using Couchbase.Services.Views;
using Couchbase.Utils;
using SequenceGenerator = Couchbase.Core.IO.Operations.SequenceGenerator;

namespace Couchbase
{
    internal interface IBucketSender
    {
        Task Send(IOperation op, TaskCompletionSource<byte[]> tcs);
    }

    public class CouchbaseBucket : IBucket, IBucketSender
    {
        internal const string DefaultScope = "_default";
        private readonly ICluster _cluster;
        private readonly ConcurrentDictionary<string, IScope> _scopes = new ConcurrentDictionary<string, IScope>();
        private BucketConfig _bucketConfig;
        private Manifest _manifest;
        internal IConnection Connection; //just for getting started
        private IKeyMapper _keyMapper;

        public CouchbaseBucket(ICluster cluster, string name)
        {
            _cluster = cluster;
            Name = name;
        }

        public string Name { get; }

        public Task<IScope> this[string name]
        {
            get
            {
                if (_scopes.TryGetValue(name, out var scope))
                {
                    return Task.FromResult(scope);
                }

                throw new ScopeNotFoundException("Cannot locate the scope {scopeName");
            }
        }

        public Task<ICollection> DefaultCollection => Task.FromResult(_scopes[DefaultScope][CouchbaseCollection.DefaultCollection]);

        public async Task BootstrapAsync(Uri uri, IConfiguration configuration)
        {
             // 1-Create a socket connection
             //2-Authenticate connection - SASL PLAIN
             // 3-Perform Select bucket
             // 4-Fetch the cluster map
             // 5-Enable server features using Helo
             // 6-Fetch the ErrorMap
             // 7-Fetch scope on demand and cache
             // 8-Fetch collection om demand and cache

            //should be abstracted
            var ipAddress = uri.GetIpAddress(false);
            var endPoint = new IPEndPoint(ipAddress, 11210);

            //#1 ----------- create a socket
            Connection = GetConnection(endPoint);

            //#2 ----------- auth the socket
            var sasl = new PlainSaslMechanism(configuration.UserName, configuration.Password);
            var authenticated = await sasl.AuthenticateAsync(Connection).ConfigureAwait(false);

            //#3 ----------- user and password are correct so perform select bucket
            if (authenticated)
            {
                var tcs = new TaskCompletionSource<bool>();
                var selectBucketOp = new SelectBucket
                {
                    Converter = new DefaultConverter(),
                    Transcoder = new DefaultTranscoder(new DefaultConverter()),
                    Key = Name,
                    Completed = s =>
                    {
                        //Status will be Success if bucket select was bueno
                        tcs.SetResult(s.Status == ResponseStatus.Success);
                        return tcs.Task;
                    }
                };
    
               await Connection.SendAsync(selectBucketOp.Write(), selectBucketOp.Completed)
                    .ConfigureAwait(false);

             //#4 ----------- fetch the cluster map
                var selected = await tcs.Task.ConfigureAwait(false);
                if (selected)
                {
                    var tcs1 = new TaskCompletionSource<byte[]>();
                    var configOp = new Config
                    {
                        CurrentHost = endPoint,
                        Converter = new DefaultConverter(),
                        Transcoder = new DefaultTranscoder(new DefaultConverter()),
                        Opaque = SequenceGenerator.GetNext(),
                       // Key = Name,
                        Completed = s =>
                        {
                            //Status will be Success if bucket select was bueno
                            tcs1.SetResult(s.Data.ToArray());
                            return tcs1.Task;
                        }
                    };

                    await Connection.SendAsync(configOp.Write(), configOp.Completed).ConfigureAwait(false);

                    var clusterMapBytes = await tcs1.Task.ConfigureAwait(false);
                    await configOp.ReadAsync(clusterMapBytes).ConfigureAwait(false);

                    var configResult = configOp.GetResultWithValue();
                    _bucketConfig = configResult.Content;              //the cluster map
                    _keyMapper = new VBucketKeyMapper(_bucketConfig);  //for vbucket key mapping
                }

                //#5----------- enable features with Helo, Helo...

                var features = new List<short>
                {
                    (short) ServerFeatures.SelectBucket,
                    (short) ServerFeatures.Collections
                };
                var tcs2 = new TaskCompletionSource<byte[]>();
                var heloOp = new Hello
                {
                    Key = Hello.BuildHelloKey(1),//temp
                    Content = features.ToArray(),
                    Converter = new DefaultConverter(),
                    Transcoder = new DefaultTranscoder(new DefaultConverter()),
                    Opaque = SequenceGenerator.GetNext(),
                    Completed = s =>
                    {
                        //Status will be Success if bucket select was bueno
                        tcs2.SetResult(s.Data.ToArray());
                        return tcs2.Task;
                    }
                };

                await Connection.SendAsync(heloOp.Write(), heloOp.Completed).ConfigureAwait(false);
                var result = await tcs2.Task.ConfigureAwait(false);
                await heloOp.ReadAsync(result).ConfigureAwait(false);
                var supported = heloOp.GetResultWithValue();

                //#6 fetch error map...

                //#7 get the manifest and cache scopes/collections
                var tcs3 = new TaskCompletionSource<byte[]>();
                var manifestOp = new GetManifest
                {
                    Converter = new DefaultConverter(),
                    Transcoder = new DefaultTranscoder(new DefaultConverter()),
                    Opaque = SequenceGenerator.GetNext(),
                    Completed = s =>
                    {
                        //Status will be Success if bucket select was bueno
                        tcs3.SetResult(s.Data.ToArray());
                        return tcs3.Task;
                    }
                };

                await Connection.SendAsync(manifestOp.Write(), manifestOp.Completed).ConfigureAwait(false);
                var manifestBytes = await tcs3.Task.ConfigureAwait(false);
                await manifestOp.ReadAsync(manifestBytes).ConfigureAwait(false);

                var manifestResult = manifestOp.GetResultWithValue();
                _manifest = manifestResult.Content;

                //warmup the scopes/collections and cache them
                foreach (var scopeDef in _manifest.scopes)
                {
                    var collections = new List<ICollection>();
                    foreach (var collectionDef in scopeDef.collections)
                    {
                        collections.Add(new CouchbaseCollection(this, collectionDef.uid, collectionDef.name));
                    }

                    _scopes.TryAdd(scopeDef.name, new Scope(scopeDef.name, scopeDef.uid, collections, this));
                }
            }
        }

        IConnection GetConnection(IPEndPoint endPoint)
        {
            var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            var waitHandle = new ManualResetEvent(false);
            var asyncEventArgs = new SocketAsyncEventArgs
            {
                RemoteEndPoint = endPoint
            };
            asyncEventArgs.Completed += delegate { waitHandle.Set(); };

            if (socket.ConnectAsync(asyncEventArgs))
            {
                // True means the connect command is running asynchronously, so we need to wait for completion

                if (!waitHandle.WaitOne(10000))//default connect timeout
                {
                    socket.Dispose();
                    const int connectionTimedOut = 10060;
                    throw new SocketException(connectionTimedOut);
                }
            }

            if ((asyncEventArgs.SocketError != SocketError.Success) || !socket.Connected)
            {
                socket.Dispose();
                throw new SocketException((int)asyncEventArgs.SocketError);
            }

            socket.SetKeepAlives(true, 2*60*60*1000, 1000);

            return new MultiplexingConnection(null, socket, new DefaultConverter());
        }

        public Task<IScope> Scope(string name)
        {
            return this[name];
        }

        public Task<IViewResult> ViewQuery<T>(string statement, IViewOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<ISpatialViewResult> SpatialViewQuery<T>(string statement, ISpatialViewOptions options)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task Send(IOperation op, TaskCompletionSource<byte[]> tcs)
        {
            var vBucket = (VBucket) _keyMapper.MapKey(op.Key);
            op.VBucketId = (short?)vBucket.Index; //hack - make vBucketIndex a short
            await Connection.SendAsync(op.Write(), op.Completed).ConfigureAwait(false);
        }
    }
}
