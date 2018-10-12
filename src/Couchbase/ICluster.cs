using System.Threading.Tasks;
using Couchbase.Services.Analytics;
using Couchbase.Services.Query;
using Couchbase.Services.Search;

namespace Couchbase
{
    public interface ICluster
    {
        IConfiguration Configuration { get; }

        Task ConnectAsync(IConfiguration config);

        IBucket GetBucket(string name);

        IQueryResponse Query(IQueryRequest request);

        IAnalyticsResponse Analyze(IAnalyticsRequest request);

        ISearchResponse Search(ISearchRequest request);
    }
}
