
namespace Couchbase
{
    public interface IDocument<TBody> : IDocument
    {
        TBody Body { get; set; }
    }
}
