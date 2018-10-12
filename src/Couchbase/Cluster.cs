using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
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

        public IConfiguration Configuration => _configuration;

        public async Task ConnectAsync(IConfiguration config)
        {
            _configuration = config;
            foreach (var configBucket in _configuration.Buckets)
            {
                foreach (var configServer in _configuration.Servers)
                {
                    try
                    {
                        var bucket = new CouchbaseBucket(this, configBucket);
                        await bucket.BootstrapAsync(configServer).ConfigureAwait(false);
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

        public IBucket GetBucket(string name)
        {
            if (_bucketRefs.TryGetValue(name, out IBucket bucket))
            {
                return bucket;
            }

            throw new ArgumentOutOfRangeException(nameof(name), "Bucket not found!");
        }

        public IQueryResponse Query(IQueryRequest request)
        {
            throw new NotImplementedException();
        }

        public IAnalyticsResponse Analyze(IAnalyticsRequest request)
        {
            throw new NotImplementedException();
        }

        public ISearchResponse Search(ISearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
