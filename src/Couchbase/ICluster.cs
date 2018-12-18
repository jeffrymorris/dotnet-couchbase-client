using System;
using System.Threading.Tasks;
using Couchbase.Core.Diagnostics;
using Couchbase.Management;
using Couchbase.Services.Analytics;
using Couchbase.Services.Query;
using Couchbase.Services.Search;
using Couchbase.Services.Views;

namespace Couchbase
{
    public interface ICluster
    {
        IConfiguration Configuration { get; }

        Task ConnectAsync(IConfiguration config);

        Task<IBucket> GetBucket(string name);

        Task<IDiagnosticsReport> Diagnostics();

        Task<IDiagnosticsReport> Diagnostics(string reportId);

        Task<IClusterManager> ClusterManager();

        Task<IQueryResult> Query<T>(string statement, QueryParameter parameters = null, IQueryOptions options = null);

        Task<IQueryResult> Query<T>(string statement, Action<QueryParameter> parameters = null,  Action<IQueryOptions> options = null);

        Task<IAnalyticsResult> AnalyzeQuery<T>(string statement, IAnalyticsOptions options);

        Task<ISearchResult> SearchQuery<T>(string statement, ISearchOptions options);

        Task<IViewResult> ViewQuery<T>(string statement, IViewOptions options);

        Task<ISpatialViewResult> SpatialViewQuery<T>(string statement, ISpatialViewOptions options);
    }
}
