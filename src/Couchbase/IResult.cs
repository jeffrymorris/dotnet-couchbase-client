
using System;
using Couchbase.Core.IO.Errors;
using Couchbase.Core.IO.Operations;

namespace Couchbase
{
    public interface IResult
    {
        ulong Cas { get; }
	
        ResponseStatus Status { get; }

        ErrorCode Error(); //from enhanced error handling

        TimeSpan? Expiration { get; }
    }
}
