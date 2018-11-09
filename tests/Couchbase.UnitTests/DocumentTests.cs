using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Couchbase.UnitTests
{
    public class DocumentTests
    {
        public async void Test_Get()
        {
            var cluster = new Cluster();
            var coll = cluster.GetBucket("default")["col1"];

            var doc = await coll.Get<dynamic>("thekey", options => options.IncludeBody = false).ConfigureAwait(false);
            var age = doc.Get<string>("age");
            doc.Upsert("age", 19);

            Assert.Null(doc.Body);


        }
    }
}
