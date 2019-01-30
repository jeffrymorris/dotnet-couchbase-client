using System;
using System.Collections.Generic;
using System.Text;
using Couchbase.Core.IO.Operations;
using Xunit;

namespace Couchbase.UnitTests
{
    public class ReadResultTests
    {
        public static byte[] PocoBytes =
        {
            129, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0,
            42, 0, 0, 0, 11, 21, 109, 149, 42, 209,
            130, 0, 0, 2, 0, 0, 1, 123, 34,
            110, 97, 109, 101, 34, 58, 34, 98, 111, 98, 34,
            44, 34, 115, 112, 101, 99, 105, 101,
            115, 34, 58, 34, 67, 97, 116, 34, 44,
            34, 97, 103, 101, 34, 58, 53, 125
        };

        public static byte[] LookupBytes =
        {
            129, 208, 0, 0, 0, 0, 0, 204,
            0, 0, 0, 30, 0, 0, 0, 10,
            21, 109, 149, 248, 110, 247, 0, 0,
            0, 0, 0, 0, 0, 7, 91, 49, 44, 50, 44,
            51, 93, 0, 0, 0, 0, 0, 5,
            34, 98, 97, 114, 34, 0, 192, 0, 0, 0, 0
        };

        [Fact]
        public void Test_FullDoc_PathRead()
        {
            var result = new GetResult(PocoBytes, "id", 0, TimeSpan.Zero, true);
            var content = result.ContentAs<string>("name");
            Assert.Equal("bob", content);
        }

        [Fact]
        public void Test_FullDoc_FullRead()
        {
            var result = new GetResult(PocoBytes, "id", 0, TimeSpan.Zero, true);
            var content = result.ContentAs<dynamic>();
            Assert.Equal("bob", content.name.Value);
        }

        
        [Fact]
        public void Test_SubDoc_PathRead()
        {
            var result = new GetResult(LookupBytes, "id", 0, TimeSpan.Zero, false);
            var content = result.ContentAs<string>("foo");
            Assert.Equal("bar", content);
        }

        [Fact]
        public void Test_SubDoc_FullRead()
        {
            var readResult = new GetResult(LookupBytes, "id", 0, TimeSpan.Zero, false);
            var content = readResult.ContentAs<dynamic>();
            Assert.Equal("bar", content["foo"]);
        }

        [Fact]
        public void Test_SubDoc_ContentAs_Entire_Doc()
        {

        }
    }
}
