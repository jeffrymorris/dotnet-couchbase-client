using System;
using Couchbase.Core.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Couchbase.UnitTests.Core.Utils
{
    public class Leb128Tests
    {
        private ITestOutputHelper _output;

        public Leb128Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(555, new byte[] {0xab, 0x04})]
        [InlineData(0x43, new byte[] {0x43})]
        [InlineData(0x5612a, new byte[] {0xAA, 0xC2, 0x15})]
        [InlineData(uint.MinValue, new byte[] {0x00})]
        [InlineData(uint.MaxValue, new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x0F})]
        public void Test_Write(uint value, byte[] expected)
        {
            var bytes = Leb128.Write(value);
            Assert.Equal(expected, bytes);

            var decoded = Leb128.Read(bytes);
            Assert.Equal(value, decoded);
        }

        [Theory]
        [InlineData("5612a", false)]
        [InlineData("0", true)]
        [InlineData("1b", true)]
        [InlineData("3356", false)]
        [InlineData("80a1", false)]
        public void Parse(string value, bool lessThan)
        {
            var cid = Convert.ToUInt32(value, 16);

            _output.WriteLine("{0} => {1}", value, cid);
            Assert.Equal(cid <= 1000, lessThan);
        }
        
        [Theory]
        [InlineData(new byte[]{ 0xab, 0x04}, 555u)]
        public void Test_Read(byte[] bytes, uint expected)
        {
            var actual = Leb128.Read(bytes);
            Assert.Equal(expected, actual);
        }
    }
}

