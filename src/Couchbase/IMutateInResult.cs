
namespace Couchbase
{
    public interface IMutateInResult : IStoreResult
    {
        T ContentAs<T>(int index);
    }
}
