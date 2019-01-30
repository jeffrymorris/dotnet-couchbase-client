using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;
using Couchbase.Core.IO.Serializers;
using Couchbase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Couchbase
{
    public class GetResult : IGetResult
    {
        private readonly byte[] _contentBytes;
        private readonly bool _isFulldoc;
        private List<OperationSpec> _specs = new List<OperationSpec>();
        private IByteConverter _converter = new DefaultConverter();
        private ITypeSerializer _serializer = new DefaultSerializer();
        
        public GetResult(string id, ulong cas, TimeSpan? expiration, bool isFulldoc)
        {
            Id = id;
            Cas = cas;
            Expiration = expiration;
            _isFulldoc = isFulldoc;
        }

        internal GetResult(byte[] contentBytes, string id, ulong cas, TimeSpan? expiration, bool isFulldoc)
        {
            Id = id;
            Cas = cas;
            Expiration = expiration;
            _contentBytes = contentBytes;
            _isFulldoc = isFulldoc;

            if (!isFulldoc)
            {
                _specs = GetValue();
                //hack to add path which isn't returned from server
                _specs[0].Path = "bar";
                _specs[1].Path = "foo";
            }
        }

        public string Id { get; }
        public ulong Cas { get; }
        public TimeSpan? Expiration { get; set; }

        public T ContentAs<T>()
        {
            if (_isFulldoc)
            {
                using (var ms = new MemoryStream(_contentBytes, 28, _contentBytes.Length - 28))
                using (var sr = new StreamReader(ms))
                using (var jr = new JsonTextReader(sr))
                {
                    var serializer = JsonSerializer.Create();
                    return serializer.Deserialize<T>(jr);
                }
            }

            return (T)(dynamic) _specs.ToDictionary(x => x.Path, y => _serializer.Deserialize<dynamic>(y.Bytes,0, y.Bytes.Length));
        }

        public T ContentAs<T>(ITypeSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public bool HasValue => _contentBytes.Length > 24;

        public T ContentAs<T>(string path)
        {
            if (_isFulldoc)
            {
                using (var ms = new MemoryStream(_contentBytes, 28, _contentBytes.Length - 28))
                using (var sr = new StreamReader(ms))
                using (var jr = new JsonTextReader(sr))
                {
                    var jObject = JObject.Load(jr);
                    var jToken = jObject.SelectToken(path);
                    return jToken.ToObject<T>();
                }   
            }

            //sub doc operation
            var spec = _specs.Find(x => x.Path == path);
           
            return _serializer.Deserialize<T>(spec.Bytes, 0, spec.Bytes.Length);
        }

        public T ContentAs<T>(string path, ITypeSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private List<OperationSpec> GetValue()
        {
            var response = _contentBytes;
            var statusOffset = 24;//Header.BodyOffset;
            var valueLengthOffset = statusOffset + 2;
            var valueOffset = statusOffset + 6;

            var operationSpecs = new List<OperationSpec>();
            for (;;)
            {
                var bodyLength = _converter.ToInt32(response, valueLengthOffset);
                var payLoad = new byte[bodyLength];
                System.Buffer.BlockCopy(response, valueOffset, payLoad, 0, bodyLength);

                var command = new OperationSpec
                {
                    Status = (ResponseStatus) _converter.ToUInt16(response, statusOffset),
                    ValueIsJson = payLoad.IsJson(0, bodyLength - 1),
                    Bytes = payLoad
                };
                operationSpecs.Add(command);

                statusOffset = valueOffset + bodyLength;
                valueLengthOffset = statusOffset + 2;
                valueOffset = statusOffset + 6;

                if (valueOffset >= response.Length) break;
            }

            return operationSpecs;
        }

    

    }
}