using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core.Configuration.Server;
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

        public CouchbaseBucket(ICluster cluster, string name)
        {
            _cluster = cluster;
            Name = name;
        }

        public string Name { get; }

        public IScope this[string name]
        {
            get
            {
                if (_scopes.TryGetValue(name, out var scope))
                {
                    return scope;
                }
            
                throw new ScopeNotFoundException("Cannot locate the scope {scopeName");
            }
        }

        public Task BootstrapAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public ICollection GetDefaultCollection()
        {
            return _scopes[DefaultScope][CouchbaseCollection.DefaultCollection];
        }

        public IScope GetScope(string name)
        {
            return this[name];
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
    }
}
