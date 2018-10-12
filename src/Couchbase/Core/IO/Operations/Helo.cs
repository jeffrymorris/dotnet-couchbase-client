using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Transcoders;
using Couchbase.Utils;
using Newtonsoft.Json;

namespace Couchbase.Core.IO.Operations
{
    public class Helo : IOperation<short[]>
    {
        const int HeaderLength = 24;

        public short[] Body { get; set; }

        public TimeSpan Timeout { get; set; }

        public uint Opaque { get; set; }

        public string Key { get; set; }

        public OpCode OpCode => OpCode.Helo;

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
            var offset = 24; //skip header and ignore extras, etc for now
            var result = new short[(bytes.Length - offset)/2];

            for (int i = 0; i < result.Length; i++)
            {
                var temp = offset + i * 2;
                if (temp < bytes.Length)
                {
                    result[i] = Converter.ToInt16(bytes, temp);
                }
            }
        }

        public byte[] Write()
        {
            var bodyBytes = new byte[Body.Length * 2];
            for (var i = 0; i < Body.Length; i++)
            {
                var offset = i * 2;
                Converter.FromInt16(Body[i], bodyBytes, offset);
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
        
        //temp location
        internal static string BuildHelloKey(ulong connectionId)
        {
            var agent = ClientIdentifier.GetClientDescription();
            if (agent.Length > 200)
            {
                agent = agent.Substring(0, 200);
            }

            return JsonConvert.SerializeObject(new
            {
                i = ClientIdentifier.FormatConnectionString(connectionId),
                a = agent
            }, Formatting.None);
        }
    }
}
