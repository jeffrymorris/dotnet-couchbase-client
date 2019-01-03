using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core.Configuration.Server;
using Couchbase.Services.Views;
using Newtonsoft.Json;

namespace Couchbase
{
    public class CouchbaseBucket : IBucket
    {
        internal const string DefaultScope = "_default";
        private readonly ICluster _cluster;
        private readonly ConcurrentDictionary<string, IScope> _scopes = new ConcurrentDictionary<string, IScope>();

        private ClusterMap _clusterMap;
        private Couchbase.Services.Collections.Manifest _manifest;
        private Task<ICollection> _defaultCollection;

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

        Task<ICollection> IBucket.DefaultCollection => _defaultCollection;

        public Task BootstrapAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection> DefaultCollection()
        {
            return Task.FromResult(_scopes[DefaultScope][CouchbaseCollection.DefaultCollection]);
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

        [Obsolete]
        public void LoadManifest(string path)
        {
            //todo fetch manifest using REST API when supported
            var jsonString = File.ReadAllText(path);
            _manifest = JsonConvert.DeserializeObject<Couchbase.Services.Collections.Manifest>(jsonString);

            foreach (var scopeDefinition in _manifest.scopes)
            {
                _scopes.TryAdd(scopeDefinition.name,
                    new Scope(scopeDefinition.name, scopeDefinition.uid,
                        scopeDefinition.collections.Select(x => new CouchbaseCollection(this, x.uid, x.name)), this));
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
