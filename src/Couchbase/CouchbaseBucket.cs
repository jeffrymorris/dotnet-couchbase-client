using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Couchbase.Core.IO.Transcoders;
using Couchbase.Utils;
using Newtonsoft.Json;

namespace Couchbase
{
    public interface IBucketSender
    {
        Task Send<T>(IOperation<T> op, TaskCompletionSource<IDocument<T>> tcs);
    }
    public class CouchbaseBucket : IBucket, IBucketSender
    {
        private const string DefaultCollection = "_default";
        private readonly ICluster _cluster;
        private readonly ConcurrentDictionary<string, List<ICollection>> _scopes = new ConcurrentDictionary<string, List<ICollection>>();
        private ClusterMap _clusterMap;
        private Couchbase.Services.Collections.Manifest _manifest;
        private IConnection _connection;

        public CouchbaseBucket(ICluster cluster, string name)
        {
            _cluster = cluster;
            Name = name;
        }

        public string Name { get; private set; }

        public async Task BootstrapAsync(Uri uri)
        {
            /*
             * 1-Create a socket connection
             * 2-Authenticate connection - SASL PLAIN
             * 3-Perform Select bucket
             * 4-Fetch the cluster map
             * 5-Enable server features using Helo
             * 6-Fetch the ErrorMap
             */

            //should be abstracted
            var ipAddress = uri.GetIpAddress(false);
            var endPoint = new IPEndPoint(ipAddress, 11210);

            //#1 ----------- create a socket
            var connection = GetConnection(endPoint);

            //#2 ----------- auth the socket
            var sasl = new PlainSaslMechanism(_cluster.Configuration.UserName, _cluster.Configuration.Password);
            var authenticated = await sasl.AuthenticateAsync(connection).ConfigureAwait(false);

            //#3 ----------- user and password are correct so perform select bucket
            if (authenticated)
            {
                var tcs = new TaskCompletionSource<bool>();
                var selectBucket = new SelectBucket
                {
                    Converter = new DefaultConverter(),
                    Transcoder = new DefaultTranscoder(new DefaultConverter()),
                    Opaque = SequenceGenerator.GetNext(),
                    Key = Name,
                    Completed = s =>
                    {
                        //Status will be Success if bucket select was bueno
                        tcs.SetResult(s.Status == ResponseStatus.Success);
                        return tcs.Task;
                    }
                };
    
               await connection.SendAsync(selectBucket.Write(), selectBucket.Completed)
                    .ConfigureAwait(false);

             //#4 ----------- fetch the cluster map
                var selected = await tcs.Task.ConfigureAwait(false);
                if (selected)
                {
                    var tcs1 = new TaskCompletionSource<byte[]>();
                    var config = new Config(endPoint)
                    {
                        Converter = new DefaultConverter(),
                        Transcoder = new DefaultTranscoder(new DefaultConverter()),
                        Opaque = SequenceGenerator.GetNext(),
                        Key = Name,
                        Completed = s =>
                        {
                            //Status will be Success if bucket select was bueno
                            tcs1.SetResult(s.Data.ToArray());
                            return tcs1.Task;
                        }
                    };

                    await connection.SendAsync(config.Write(), config.Completed).ConfigureAwait(false);

                    var clusterMapBytes = await tcs1.Task.ConfigureAwait(false);
                    config.Read(clusterMapBytes);
                    _clusterMap = config.Body;
                }

                //#5----------- enable features with Helo, Helo...

                var features = new List<short>
                {
                    (short) ServerFeatures.SelectBucket,
                    (short) ServerFeatures.Collections
                };
                var tcs2 = new TaskCompletionSource<byte[]>();
                var helo = new Helo
                {
                    Key = Helo.BuildHelloKey(1),//temp
                    Body = features.ToArray(),
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

                await connection.SendAsync(helo.Write(), helo.Completed).ConfigureAwait(false);
                var result = await tcs2.Task.ConfigureAwait(false);
                helo.Read(result);

                //#6 fetch error map...

                _connection = connection;//temp hack
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

        public ICollection this[string name] => GetCollection(DefaultCollection, name);

        public ICollection GetCollection(string name)
        {
            return null;
        }

        public ICollection GetCollection(string scope, string name)
        {
            if(_scopes.TryGetValue(scope, out List<ICollection> collections))
            {
                var collection = collections.FirstOrDefault(x=>x.Name==name);
                if (collection == null)
                {
                    throw new Exception("Collection not found: " + name);//todo make exception more specific
                }
                return collection;
            }
            throw new Exception("Scope not found: " + scope);//todo make exception more specific
        }

        public IMutationOperation Upsert<T>(IDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation Insert<T>(IDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation Replace<T>(IDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation Remove<T>(IDocument<T> document)
        {
            throw new NotImplementedException();
        }

        public IFetchOperation<T> Get<T>(string key)
        {
            return new Get<T>(this)
            {
                Converter = new DefaultConverter(),
                Transcoder = new DefaultTranscoder(new DefaultConverter()),
                Opaque = SequenceGenerator.GetNext(),
                Key = key
            };
        }

        public void LoadManifest(string path)
        {
            //todo fetch manifest using REST API when supported
            var jsonString = File.ReadAllText(path);
            _manifest = JsonConvert.DeserializeObject<Couchbase.Services.Collections.Manifest>(jsonString);

            foreach (var scope in _manifest.scopes)
            {
                _scopes.TryAdd(scope.name, new List<ICollection>());
                foreach (var collection in scope.collections)
                {
                    _scopes[scope.name].Add(new Collection(this, collection.uid, collection.name));
                }
            }
        }

        async Task IBucketSender.Send<T>(IOperation<T> op, TaskCompletionSource<IDocument<T>> tcs)
        {
            await this._connection.SendAsync(op.Write(), op.Completed);
            await tcs.Task;
        }
    }
}
