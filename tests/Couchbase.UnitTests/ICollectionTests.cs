using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Couchbase.UnitTests
{
    public class ICollectionTests
    {
        [Fact]
        public void Test_Func()
        {
            var c = new Collection(null, "ss", "ss");
            var fetch = c.Get<dynamic>("bar", options => { options.Timeout(new TimeSpan(0, 0, 1)); });

        }
    }
}
