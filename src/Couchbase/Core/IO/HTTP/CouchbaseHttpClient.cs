using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Couchbase.Utils;

namespace Couchbase.Core.IO.HTTP
{
    public class CouchbaseHttpClient : HttpClient
    {
        private const string UserAgentHeaderName = "User-Agent";
    }
}
