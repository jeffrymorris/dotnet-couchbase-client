using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Core.Diagnostics;
using Couchbase.Management;
using Couchbase.Services.Analytics;
using Couchbase.Services.Query;
using Couchbase.Services.Search;

namespace Couchbase
{
    public class Cluster : ICluster
    {
        private readonly ConcurrentDictionary<string, IBucket> _bucketRefs = new ConcurrentDictionary<string, IBucket>();
        private IConfiguration _configuration;

        public Cluster()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IBucket> this[string name] => throw new NotImplementedException();

        public Task<IBucket> Bucket(string name)
        {
            if (_bucketRefs.TryGetValue(name, out IBucket bucket))
            {
                return Task.FromResult(bucket);
            }

            throw new ArgumentOutOfRangeException(nameof(name), "Bucket not found!");
        }

        public Task<IDiagnosticsReport> Diagnostics()
        {
            throw new NotImplementedException();
        }

        public Task<IDiagnosticsReport> Diagnostics(string reportId)
        {
            throw new NotImplementedException();
        }

        public Task<IClusterManager> ClusterManager()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult<T>> Query<T>(string statement, QueryParameter parameters = null, IQueryOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult<T>> Query<T>(string statement, Action<QueryParameter> parameters = null, Action<IQueryOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IAnalyticsResult> AnalyticsQuery<T>(string statement, IAnalyticsOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<ISearchResult> SearchQuery<T>(ISearchQuery query, ISearchOptions options)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Temp")]
        public async Task ConnectAsync(IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.Password) || string.IsNullOrWhiteSpace(config.UserName))
            {
                throw new ArgumentNullException(nameof(config), "Username and password are required.");
            }

            _configuration = config;
            if (!_configuration.Buckets.Any())
            {
                _configuration = _configuration.WithBucket("default");
                _configuration = _configuration.WithServers("couchbase://localhost");
            }

            foreach (var configBucket in _configuration.Buckets)
            {
                foreach (var configServer in _configuration.Servers)
                {
                    try
                    {
                        var bucket = new CouchbaseBucket(this, configBucket);
                        //await bucket.BootstrapAsync(configServer).ConfigureAwait(false);
                        bucket.LoadManifest("manifest.json"); //just for testing UI - guts coming soon
                        _bucketRefs.TryAdd(configBucket, bucket);
                        return;
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
