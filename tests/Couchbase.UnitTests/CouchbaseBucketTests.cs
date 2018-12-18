using System.Threading.Tasks;
using Xunit;

namespace Couchbase.UnitTests
{
    public class CouchbaseBucketTests
    {
        [Fact]
        public async Task GetDefaultCollection_Returns_Default_Collection()
        {
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = cluster.GetBucket("default");
            var collection = bucket.GetDefaultCollection();

            Assert.NotNull(collection);
            Assert.Equal("_default", collection.Name);
        }

        [Fact]
        public async Task GetScope_Returns_Scope()
        {
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = cluster.GetBucket("default");
            var scope = bucket.GetScope("app_west");

            Assert.NotNull(scope);
            Assert.Equal("app_west", scope.Name);
            Assert.Equal("43", scope.Id);
        }

        [Fact]
        public async Task GetScope_Throws_ScopeNotFoundException_When_Scope_Not_Found()
        {
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = cluster.GetBucket("default");
            Assert.Throws<ScopeNotFoundException>(()=> bucket.GetScope("xyz"));
        }
    }
}
