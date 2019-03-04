using System;
using System.Threading.Tasks;
using Couchbase.UnitTests.Fixtures;
using Xunit;

namespace Couchbase.UnitTests
{
    public class GetTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _fixture;

        public GetTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Can_get_document()
        {
            var collection = await _fixture.GetDefaultCollection();
            var key = Guid.NewGuid().ToString();

            try
            {
                await collection.Insert(key, new {name = "mike"});

                var result = await collection.Get(key);
                var content = result.ContentAs<dynamic>();

                Assert.Equal("mike", (string) content.name);
            }
            finally
            {
                await collection.Remove(key);
            }
        }
    }
}
