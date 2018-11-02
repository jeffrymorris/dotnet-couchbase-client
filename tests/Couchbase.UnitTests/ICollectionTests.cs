using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Mock;
using Xunit;

namespace Couchbase.UnitTests
{
    public class ICollectionTests
    {
        [Fact]
        public void Get_Test()
        {
            //var cluster = new Mock<>

            var c = new CouchbaseCollection(null, "ss", "ss");
            //var fetch = c.Get<dynamic>("bar", options => { options.Timeout(new TimeSpan(0, 0, 1)); });
            var fetch = c.Get<dynamic>("bar", options => options.Timeout = new TimeSpan(0, 0, 1));
        }
    }
}
