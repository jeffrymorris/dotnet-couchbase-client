
using System.Threading.Tasks;

namespace Couchbase
{
    public interface IDocument<TBody> : IDocument
    {
        TBody Body { get; set; }

        TBody GetBody();

        Task SetBody(TBody json);
    }
}
