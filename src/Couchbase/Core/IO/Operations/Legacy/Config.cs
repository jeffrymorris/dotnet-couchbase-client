using System;
using System.Net;
using Couchbase.Core.Configuration.Server;
using Couchbase.Core.Transcoders;
using Newtonsoft.Json;

namespace Couchbase.Core.IO.Operations.Legacy
{
    internal sealed class Config : OperationBase<ClusterMap>
    {
        private readonly IPEndPoint _endpoint;

        public Config(ITypeTranscoder transcoder, uint timeout, IPEndPoint endpoint)
            : this(string.Empty, null, transcoder, timeout, endpoint)
        {
        }

        public Config(string key, ClusterMap value, ITypeTranscoder transcoder, IVBucket vBucket, uint opaque, uint timeout, IPEndPoint endpoint)
            : base(key, value, vBucket, transcoder, opaque, timeout)
        {
            _endpoint = endpoint;
        }

        public Config(string key, IVBucket vBucket, ITypeTranscoder transcoder, uint timeout, IPEndPoint endpoint)
            : base(key, vBucket, transcoder, timeout)
        {
            _endpoint = endpoint;
        }

        public override byte[] CreateExtras()
        {
            Format = DataFormat.Json;
            Flags = new Flags
            {
                Compression = Compression.None,
                DataFormat = Format,
                TypeCode = TypeCode.Object
            };
            return new byte[0];
        }

        public override void ReadExtras(byte[] buffer)
        {
            Format = DataFormat.String;
            Flags = new Flags
            {
                Compression = Compression.None,
                DataFormat = Format,
                TypeCode = TypeCode.Object
            };
        }

        public override OpCode OpCode => OpCode.GetClusterConfig;

        public override ClusterMap GetValue()
        {
            ClusterMap bucketConfig = null;
            if (Success && Data != null)
            {
                try
                {
                    var buffer = Data.ToArray();
                    ReadExtras(buffer);
                    var offset = Header.BodyOffset;
                    var length = TotalLength - Header.BodyOffset;
                    var json = Transcoder.Decode<string>(buffer, offset, length, Flags, OpCode);
                    if (_endpoint != null)
                    {
                        json = json.Replace("$HOST", _endpoint.Address.ToString());
                    }
                    bucketConfig = JsonConvert.DeserializeObject<ClusterMap>(json);
                }
                catch (Exception e)
                {
                    Exception = e;
                    HandleClientError(e.Message, ResponseStatus.ClientFailure);
                }
            }
            return bucketConfig;
        }

        public override bool RequiresKey => false;
    }
}

#region [ License information ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2014 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion [ License information ]
