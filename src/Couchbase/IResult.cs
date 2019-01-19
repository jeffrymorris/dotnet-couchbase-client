using System;

namespace Couchbase
{
    public interface IResult
    {
        ulong Cas { get; }
	
        //ResponseStatus Status { get; }

        //ErrorCode Error(); //from enhanced error handling

        TimeSpan? Expiration { get; }
    }
}
