
namespace Couchbase
{
    public interface IDocument<T> : IDocument
    {
        T Content { get; set; }
    }
}
