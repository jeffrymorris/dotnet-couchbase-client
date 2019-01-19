
using System;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase.Core.IO.Operations
{
    public interface IOperation
    {

    }

    public interface IOperation<T>
    {
        T Body { get; set; }

        TimeSpan Timeout { get; set; }

        uint Opaque { get; set; }

        string Key { get; set; }

        OpCode OpCode { get; }

        Flags Flags { get; set; }

        IByteConverter Converter { get; set; }

        ITypeTranscoder Transcoder { get; set; }

        int? VBucketIndex { get; set; }

        Func<SocketAsyncState, Task> Completed { get; set; }

        byte[] Write();

        void Read(byte[] bytes);
    }
}
