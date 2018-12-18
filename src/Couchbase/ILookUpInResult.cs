using System;
using Couchbase.Core.IO.Serializers;

namespace Couchbase
{
    public interface ILookupInResult
    {
        string Id { get; }

        ulong Cas { get; set; }

        TimeSpan Expiry { get; }

        bool Exists(int index);

        T ContentAs<T>(int index);

        T ContentAs<T>(int index, ITypeSerializer serializer);
    }
}
