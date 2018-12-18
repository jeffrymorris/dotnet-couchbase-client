
namespace Couchbase
{
    public interface ICounterResult : IStoreResult
    {
        ulong Value { get; set; }
    }
}
