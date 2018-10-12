using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Errors;
using Couchbase.Core.IO.Operations;

namespace Couchbase.Core.IO
{
     /// <summary>
    /// Represents an asynchronous Memcached request in flight.
    /// </summary>
    internal class AsyncState : IState
    {
        public IPEndPoint EndPoint { get; set; }
        public Func<SocketAsyncState, Task> Callback { get; set; }
        public IByteConverter Converter { get; set; }
        public uint Opaque { get; set; }
        public Timer Timer; 
        public string ConnectionId { get; set; }
        public ErrorMap ErrorMap { get; set; }
        public int Timeout { get; set; }
        public string LocalEndpoint { get; set; }

        /// <summary>
        /// Cancels the current Memcached request that is in-flight.
        /// </summary>
        public void Cancel(ResponseStatus status, Exception e = null)
        {
            if (Timer != null)
            {
                Timer.Dispose();
            }

            var response = new byte[24];
            Converter.FromUInt32(Opaque, response, HeaderOffsets.Opaque);

            Callback(new SocketAsyncState
            {
                Data = new MemoryStream(response),
                Opaque = Opaque,
                // ReSharper disable once MergeConditionalExpression
                Exception = e,
                Status = status,
                EndPoint = EndPoint,
                ConnectionId = ConnectionId,
                ErrorMap = ErrorMap,
                Timeout = Timeout,
                LocalEndpoint = LocalEndpoint
            });
        }

        /// <summary>
        /// Completes the specified Memcached response.
        /// </summary>
        /// <param name="response">The Memcached response packet.</param>
        public void Complete(byte[] response)
        {
            Timer?.Dispose();

            //defaults
            var status = (ResponseStatus) Converter.ToInt16(response, HeaderOffsets.Status);
            Exception e = null;

            //this means the request never completed - assume a transport failure
            if (response == null)
            {
                response = new byte[24];
                Converter.FromUInt32(Opaque, response, HeaderOffsets.Opaque);
                e = new Exception("SendTimeoutException");
                status = ResponseStatus.TransportFailure;
            }

            //somewhat of hack for backwards compatibility
            Task.Run(() => Callback(new SocketAsyncState
            {
                Data = new MemoryStream(response),
                Opaque = Opaque,
                Exception = e,
                Status = status,
                EndPoint = EndPoint,
                ConnectionId = ConnectionId,
                ErrorMap = ErrorMap,
                Timeout = Timeout,
                LocalEndpoint = LocalEndpoint
            }));
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
