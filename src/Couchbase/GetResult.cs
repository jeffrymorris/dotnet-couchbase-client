﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Core.IO.Operations.SubDocument;
using Couchbase.Core.IO.Serializers;
using Couchbase.Core.IO.Transcoders;
using Couchbase.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Couchbase
{
    public class GetResult : IGetResult
    {
        private readonly byte[] _contentBytes;
        private readonly List<OperationSpec> _specs;
        private readonly ITypeTranscoder _transcoder;
        private ITypeSerializer _serializer;
        private readonly IByteConverter _converter;

        internal GetResult(byte[] contentBytes, ITypeTranscoder transcoder, List<OperationSpec> specs = null)
        {
            _contentBytes = contentBytes;
            _transcoder = transcoder;
            _serializer = transcoder.Serializer;
            _converter = transcoder.Converter;
            _specs = specs;
        }

        internal OperationHeader Header { get; set; }
        internal OpCode OpCode { get; set; }
        internal Flags Flags { get; set; }

        public string Id { get; internal set; }
        public ulong Cas { get; internal set; }
        public TimeSpan? Expiration { get; set; }

        public T ContentAs<T>()
        {
            //basic GET operation
            if (OpCode == OpCode.Get)
            {
                var bodyOffset = Header.BodyOffset;
                var length = _contentBytes.Length - Header.BodyOffset;
                return _transcoder.Decode<T>(_contentBytes, bodyOffset, length, Flags, OpCode);
            }

            //oh mai, its a projection
            ParseSpecs();

            var root = new JObject();
            foreach (var spec in _specs)
            {
                var content = _serializer.Deserialize<JToken>(spec.Bytes, 0, spec.Bytes.Length);
                var projection = CreateProjection(spec.Path, content);

                try
                {
                    root.Add(projection.First); //hacky should be improved later
                }
                catch (Exception e)
                {
                    //ignore for now
                }
            }
            return root.ToObject<T>();
        }

        void AddToRoot(JObject jObject)
        {

        }
        public T ContentAs<T>(ITypeSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public bool HasValue => _contentBytes.Length > 24;

        private void ParseSpecs()
        {
            var response = _contentBytes;
            var statusOffset = Header.BodyOffset;
            var valueLengthOffset = statusOffset + 2;
            var valueOffset = statusOffset + 6;
            var commandIndex = 0;

            for (;;)
            {
                var bodyLength = _converter.ToInt32(response, valueLengthOffset);
                var payLoad = new byte[bodyLength];
                Buffer.BlockCopy(response, valueOffset, payLoad, 0, bodyLength);

                var command = _specs[commandIndex++];
                command.Status = (ResponseStatus)_converter.ToUInt16(response, statusOffset);
                command.ValueIsJson = payLoad.IsJson(0, bodyLength - 1);
                command.Bytes = payLoad;

                statusOffset = valueOffset + bodyLength;
                valueLengthOffset = statusOffset + 2;
                valueOffset = statusOffset + 6;

                if (valueOffset >= response.Length) break;
            }
        }

        void BuildPath(JToken token, string name, JToken content =  null)
        {
            foreach (var child in token.Children())
            {
                if (child is JValue)
                {
                    var value = child as JValue;
                    value.Replace(new JObject(new JProperty(name, content)));
                    break;
                }
                BuildPath(child, name, content);
            }
        }

        JObject CreateProjection(string path, JToken content)
        {
            var elements = path.Split(".");
            var projection = new JObject();
            if (elements.Length == 1)
            {
                projection.Add(new JProperty(elements[0], content));
            }
            else
            {
                for (var i = 0; i < elements.Length; i++)
                {
                    if (projection.Last != null)
                    {
                        if (i == elements.Length - 1)
                        {
                            BuildPath(projection, elements[i], content);
                            continue;
                        }

                        BuildPath(projection, elements[i]);
                        continue;
                    }

                    projection.Add(new JProperty(elements[i], null));
                }
            }

            return projection;
        }
    }
}