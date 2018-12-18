
namespace Couchbase.Core.IO.Operations.Temp
{
    interface IOperation
    {
        IHeader Header { get; set; }

        IBody Body { get; set; }

        IOperationResponse GetResponse();
    }
}
