using System;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase.Core.IO.Operations
{
    public class SelectBucket : IOperation<object>
    {
        const int HeaderLength = 24;
        public object Body { get; set; }
        public TimeSpan Timeout { get; set; }
        public uint Opaque { get; set; }
        public string Key { get; set; }
        public OpCode OpCode => OpCode.SelectBucket;

        public Flags Flags { get; set; }
        public IByteConverter Converter { get; set; }
        public ITypeTranscoder Transcoder { get; set; }
        public int? VBucketIndex { get; set; }
        public Func<SocketAsyncState, Task> Completed { get; set; }
        public byte[] Write()
        {
            byte[] bodyBytes = new byte[0];
            var keyBytes = Encoding.UTF8.GetBytes(Key);
            var extras = new byte[0];

            var headerBytes = new byte[HeaderLength];
            Converter.FromByte((byte)Magic.Request, headerBytes, HeaderOffsets.Magic);
            Converter.FromByte((byte)OpCode, headerBytes, HeaderOffsets.Opcode);
            Converter.FromInt16((short)keyBytes.Length, headerBytes, HeaderOffsets.KeyLength);
            Converter.FromInt32(keyBytes.Length + bodyBytes.Length, headerBytes, HeaderOffsets.BodyLength);
            Converter.FromUInt32(Opaque, headerBytes, HeaderOffsets.Opaque);

            var buffer = new byte[headerBytes.Length + bodyBytes.Length +extras.Length + keyBytes.Length];
                              
            System.Buffer.BlockCopy(headerBytes, 0, buffer, 0, headerBytes.Length);
            System.Buffer.BlockCopy(extras, 0, buffer, headerBytes.Length, extras.Length);
            System.Buffer.BlockCopy(keyBytes, 0, buffer, headerBytes.Length + extras.Length, keyBytes.Length);
            System.Buffer.BlockCopy(bodyBytes, 0, buffer, headerBytes.Length + extras.Length + keyBytes.Length, bodyBytes.Length);

            return buffer;
        }

        public void Read(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
