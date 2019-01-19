using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Couchbase.Core.Configuration.Server;
using Couchbase.Core.IO.Operations.Legacy.Errors;
using Couchbase.Core.Transcoders;

namespace Couchbase.Core.IO.Operations.Legacy
{
    public interface IOperation
    {
        OpCode OpCode { get; }

        string Key { get; }

        bool RequiresKey { get; }

        Exception Exception { get; set; }

        int BodyOffset { get; }

        ulong Cas { get; set; }

        void Read(byte[] buffer, ErrorMap errorMap = null);

        void Read(byte[] buffer, OperationHeader header, ErrorCode errorCode);

        byte[] Write();

        Task<byte[]> WriteAsync();

        MemoryStream Data { get; set; }

        byte[] Buffer { get; set; }

        int LengthReceived { get; }

        int TotalLength { get; }

        string GetMessage();

        void Reset();

        OperationHeader Header { get; set; }

        OperationBody Body { get; set; }

        int Attempts { get; set; }

        int MaxRetries { get; }

        IVBucket VBucket { get; set; }

        void HandleClientError(string message, ResponseStatus responseStatus);

       ClusterMap GetConfig(ITypeTranscoder transcoder);

        uint Opaque { get; }

        uint Timeout { get; set; }

        bool TimedOut();

        DateTime CreationTime { get; set; }

        Task ReadAsync(byte[] buffer, ErrorMap errorMap = null);

        Task ReadAsync(byte[] buffer, OperationHeader header, ErrorCode errorCode);

        byte[] WriteBuffer { get; set; }

        Func<SocketAsyncState, Task> Completed { get; set; }

        bool CanRetry();

        IOperationResult GetResult();

        IPEndPoint CurrentHost { get; set; }

        IOperation Clone();

        uint LastConfigRevisionTried { get; set; }

        string BucketName { get; set; }

        int GetRetryTimeout(int defaultTimeout);
    }
}

#region [ License information          ]

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

#endregion [ License information          ]
