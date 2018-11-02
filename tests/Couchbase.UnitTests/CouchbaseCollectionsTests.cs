using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Couchbase.UnitTests
{
    public class CouchbaseCollectionsTests
    {
        [Fact]
        public void Test()
        {
            var coll = new CouchbaseCollection(null, "", "");
            coll.Get<dynamic>("id", options => { options.Timeout = (new TimeSpan(0, 0, 30));});
        }
    }
}
