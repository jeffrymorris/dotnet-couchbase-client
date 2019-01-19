using System;

namespace Couchbase.Core.IO
{
    public struct Flags
    {
        public DataFormat DataFormat { get; set; }

        public Compression Compression { get; set; }

        public TypeCode TypeCode { get; set; }
    }
}
