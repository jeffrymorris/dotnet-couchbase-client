using System;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Serializers;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase.Core.IO.Operations
{
    public class Get<T> : IFetchOperation<T>
    {
        const int HeaderLength = 24;
        private IBucket _bucket;

        public Get(IBucket bucket)
        {
            _bucket = bucket;
        }

        public IFetchOperation<T> WithExpiry(TimeSpan expiry)
        {
            throw new NotImplementedException();
        }

        public IFetchOperation<T> WithTimeout(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IFetchOperation<T> WithCas(uint cas)
        {
            throw new NotImplementedException();
        }

        public IFetchOperation<T> WithDurability(PersistTo peristTo, ReplicateTo replicateTo)
        {
            throw new NotImplementedException();
        }

        public IFetchOperation<T> WithTranscoder(ITranscoder transcoder)
        {
            throw new NotImplementedException();
        }


        public IFetchOperation<T> WithSerializer(ITypeSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument_T<T>> ExecuteAsync()
        {
            var tcs = new TaskCompletionSource<IDocument_T<T>>();
            Completed = s =>
            {
                //Status will be Success if bucket select was bueno
                var result = s.Data.ToArray();
                tcs.SetResult(null);
                return tcs.Task;
            };
            (_bucket as IBucketSender).Send(this, tcs);
            return tcs.Task;
        }

        //basic memcached properties
        public T Body { get; set; }
        public TimeSpan Timeout { get; set; }
        public uint Opaque { get; set; }
        public string Key { get; set; }
        public OpCode OpCode => OpCode.Get;
        public Flags Flags { get; set; }
        public IByteConverter Converter { get; set; }
        public ITypeTranscoder Transcoder { get; set; }
        public string Cid { get; set; }

        public int? VBucketIndex
        {
            get => 389; //hard coded for testing
            set { }
        }
        public Func<SocketAsyncState, Task> Completed { get; set; }

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

            var keyBytes = Encoding.UTF8.GetBytes(Key);
            var extras = new byte[0];

            var headerBytes = new byte[HeaderLength];
            Converter.FromByte((byte)Magic.Request, headerBytes, HeaderOffsets.Magic);
            Converter.FromByte((byte)OpCode, headerBytes, HeaderOffsets.Opcode);
            Converter.FromInt16((short)keyBytes.Length, headerBytes, HeaderOffsets.KeyLength);
            Converter.FromByte((byte)extras.Length, headerBytes, HeaderOffsets.ExtrasLength);

            if (VBucketIndex.HasValue)
            {
                Converter.FromInt16((short)VBucketIndex, headerBytes, HeaderOffsets.VBucket);
            }

            Converter.FromInt32(keyBytes.Length + bodyBytes.Length, headerBytes, HeaderOffsets.BodyLength);
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