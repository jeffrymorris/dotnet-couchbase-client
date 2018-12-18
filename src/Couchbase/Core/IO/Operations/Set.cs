using System;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Transcoders;
using Couchbase.Core.Utils;

namespace Couchbase.Core.IO.Operations
{
    public class Set<T> : IMutationOperation, IOperation<T>
    {
        const int HeaderLength = 24;
        private IBucket _bucket;
        private IDocument _document;
        private uint _expiry;

        public Set(IBucket bucket, IDocument document)
        {
            _bucket = bucket;
            _document = document;
        }

        public IMutationOperation WithExpiry(TimeSpan expiry)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithTimeout(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithCas(uint cas)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithDurability(PersistTo peristTo, ReplicateTo replicateTo)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync()
        {
            var tcs = new TaskCompletionSource<IDocument>();
            Completed = s =>
            {
                //Status will be Success if bucket select was bueno
                var result = s.Data.ToArray();
                tcs.SetResult(null);
                return tcs.Task;
            };
            //(_bucket as IBucketSender).Send(this, tcs);
            return tcs.Task;
        }

        public T Body { get; set; }
        public TimeSpan Timeout { get; set; }
        public uint Opaque { get; set; }
        public string Key
        {
            get => _document.Id;
            set => _document.Id = value;
        }

        public OpCode OpCode => OpCode.Add;
        public Flags Flags { get; set; }
        public IByteConverter Converter { get; set; }
        public ITypeTranscoder Transcoder { get; set; }
        public int? VBucketIndex
        {
            get => 389; //hard coded for testing
            set { }
        }
        public Func<SocketAsyncState, Task> Completed { get; set; }
        public string Cid 
        {
            get=>"0x3356";
            set { }
        }

        public byte[] Write()
        {
            byte[] bodyBytes;
            if (Body != null && Body.GetType().IsValueType)
            {
                bodyBytes = Transcoder.Encode(Body, Flags, OpCode);
            }
            else
            {
                bodyBytes = Body== null ? new byte[0] :
                    Transcoder.Encode(Body, Flags, OpCode);
            }

            var keyBytesCount = Encoding.UTF8.GetByteCount(Key) + 2;
            var keyBytes = new byte[keyBytesCount]; //two more for collection CID todo make not hardcoded
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(Key), 0, keyBytes, 2, keyBytesCount-2);

            var temp = Convert.ToUInt32(Cid, 16); //hex for now - leb translationn should happen one time when mani is fetched
            var lebCid = Leb128.Write(temp, 2);
            
            Buffer.BlockCopy(lebCid, 0, keyBytes, 0, lebCid.Length);
            
            var extras = new byte[8];
            Flags = Transcoder.GetFormat<T>(Body);
            var Format = Flags.DataFormat;
            var Compression = Flags.Compression;

            byte format = (byte)Format;
            byte compression = (byte)Compression;

            Converter.SetBit(ref extras[0], 0, Converter.GetBit(format, 0));
            Converter.SetBit(ref extras[0], 1, Converter.GetBit(format, 1));
            Converter.SetBit(ref extras[0], 2, Converter.GetBit(format, 2));
            Converter.SetBit(ref extras[0], 3, Converter.GetBit(format, 3));
            Converter.SetBit(ref extras[0], 4, false);
            Converter.SetBit(ref extras[0], 5, Converter.GetBit(compression, 0));
            Converter.SetBit(ref extras[0], 6, Converter.GetBit(compression, 1));
            Converter.SetBit(ref extras[0], 7, Converter.GetBit(compression, 2));

            var typeCode = (ushort)Flags.TypeCode;
            Converter.FromUInt16(typeCode, extras, 2);
            Converter.FromUInt32(_expiry, extras, 4);

            var headerBytes = new byte[HeaderLength];
            Converter.FromByte((byte) Magic.Request, headerBytes, HeaderOffsets.Magic);
            Converter.FromByte((byte) OpCode, headerBytes, HeaderOffsets.Opcode);
            Converter.FromInt16((short) keyBytes.Length, headerBytes, HeaderOffsets.KeyLength);
            Converter.FromByte((byte) extras.Length, headerBytes, HeaderOffsets.ExtrasLength);        

            if (VBucketIndex.HasValue)
            {
                Converter.FromInt16((short)VBucketIndex, headerBytes, HeaderOffsets.VBucket);
            }

            Converter.FromInt32(keyBytes.Length + bodyBytes.Length + extras.Length, headerBytes, HeaderOffsets.BodyLength);
            Converter.FromUInt32(Opaque, headerBytes, HeaderOffsets.Opaque);
            //Converter.FromUInt64(Cas, header, HeaderIndexFor.Cas);     

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
