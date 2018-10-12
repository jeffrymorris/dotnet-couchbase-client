using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase.Core.IO.Operations
{
    public class Manifest : IOperation<string>
    {
        const int HeaderLength = 24;

        public string Body { get; set; }

        public TimeSpan Timeout { get; set; }

        public uint Opaque { get; set; }

        public string Key { get; set; }

        public OpCode OpCode => OpCode.GetCollectionsManifest;

        public Flags Flags {
            get => new Flags
            {
                Compression = Compression.None,
                DataFormat = DataFormat.String,
                TypeCode = TypeCode.String
            };
            set => throw new NotImplementedException();
        }

        public IByteConverter Converter { get; set; }

        public ITypeTranscoder Transcoder { get; set; }
        public int? VBucketIndex { get; set; }

        public Func<SocketAsyncState, Task> Completed { get; set; }

        public void Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] Write()
        {
            throw new NotImplementedException();
        }
    }
}
