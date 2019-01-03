using System.Threading.Tasks;
using Xunit;

namespace Couchbase.UnitTests
{
    public class ScopeTests
    {   
        [Fact]
        public async Task GetCollection_Returns_Collection()
        {
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = await cluster.Bucket("default");
            var scope = await bucket.Scope("app_west");
            var collection = scope.GetCollection("users");

            Assert.NotNull(collection);
            Assert.Equal("users", collection.Name);
        }

        [Fact]
        public async Task GetCollection_Throws_CollectionNotFoundException_When_Collection_Missing()
        {
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = await cluster.Bucket("default");
            var scope = await bucket.Scope("app_west");
           
            Assert.Throws<CollectionNotFoundException>(()=> scope.GetCollection("xyz"));
        }
    }
}
