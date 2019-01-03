using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Couchbase.IntegrationTests
{
    public class CouchbaseBucketTests
    {
       [Fact]
        public void Test_Bootstrap()
        {
            var ip = "http://10.142.161.102:8091";
            var cluster = new Cluster();
        }
    }
}
