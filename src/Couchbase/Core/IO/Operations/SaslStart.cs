using System;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase.Core.IO.Operations
{
    
    /*Field (offset) (value)
        Magic (0): 0x80 (PROTOCOL_BINARY_REQ)
        Opcode (1): 0x21 (sasl auth)
        Key length (2-3): 0x0005 (5)
        Extra length (4): 0x00
        Data type (5): 0x00
        vBucket (6-7): 0x0000 (0)
        Total body (8-11): 0x00000010 (16)
        Opaque (12-15): 0x00000000 (0)
        CAS (16-23): 0x0000000000000000 (0)
        Mechanisms (24-28): PLAIN
        Auth token (29-39): foo0x00foo0x00bar
    */

    public class SaslStart : IOperation<string>
    {
        const int HeaderLength = 24;

        public string Body { get; set; }
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public uint Opaque { get; set; }

        public string Key { get; set; }

        public OpCode OpCode => OpCode.SaslStart;

        public Flags Flags
        {
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
        Flags IOperation<string>.Flags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] Write()
        {
            byte[] bodyBytes;
            if (Body.GetType().IsValueType)
            {
                bodyBytes = Transcoder.Encode(Body, Flags, OpCode);
            }
            else
            {
                bodyBytes = Body== null ? new byte[0] :
                    Transcoder.Encode(Body, Flags, OpCode);
            }

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
