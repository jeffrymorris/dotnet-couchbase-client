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

        Task<IBucket> GetBucket(string name);

        Task<IQueryResponse<T>> Query<T>(IQueryRequest request);

        Task<IAnalyticsResponse<T>> Analyze<T>(IAnalyticsRequest request);

        Task<ISearchResponse<T>> Search<T>(ISearchRequest request);
    }
}
