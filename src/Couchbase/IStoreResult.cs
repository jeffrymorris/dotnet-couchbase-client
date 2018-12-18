
using Couchbase.Core;

namespace Couchbase
{
    public interface IStoreResult
    {
        ulong Cas { get; }

        MutationToken MutationToken { get; set; }
    }
}
