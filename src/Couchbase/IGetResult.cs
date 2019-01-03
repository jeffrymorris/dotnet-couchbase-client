using System;
using Couchbase.Core.IO.Serializers;

namespace Couchbase
{
    public interface IGetResult
    {  
        string Id { get; }

        ulong? Cas { get; }

        TimeSpan? Expiration { get; set; }

        T ContentAs<T>();

        T ContentAs<T>(ITypeSerializer serializer);
    }
}
