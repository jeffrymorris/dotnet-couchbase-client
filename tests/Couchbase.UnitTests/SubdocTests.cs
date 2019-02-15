using System;
using System.Threading.Tasks;
using Xunit;

namespace Couchbase.UnitTests
{
    public class SubdocTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _fixture;

        public SubdocTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        private const string key = "document-key";

        [Fact]
        public async Task Can_perform_lookup_in()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(key, new {foo = "bar", bar = "foo"});

            var result = await collection.LookupIn(key, ops =>
            {
                ops.Path("foo");
                ops.Path("bar");
            });

            Assert.Equal("bar", result.ContentAs<string>(0));
            Assert.Equal("foo", result.ContentAs<string>(1));
        }

        [Fact]
        public async Task Can_perform_mutate_in()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(key, new {foo = "bar", bar = "foo"});

            await collection.MutateIn(key, ops =>
            {
                ops.Upsert("name", "mike");
            });

            var getResult = await collection.Get(key);
            var content = getResult.ContentAs<dynamic>();
        }
    }

    public class ClusterFixture : IDisposable
    {
        public ICluster Cluster { get; }

        public ClusterFixture()
        {
            var cluster = new Cluster();
            var task = cluster.Initialize(
                new Configuration()
                    .WithServers("couchbase://10.112.193.101")
                    .WithBucket("default")
                    .WithCredentials("Administrator", "password")
            );
            task.ConfigureAwait(false);
            task.Wait();

            Cluster = cluster;
        }

        public async Task<IBucket> GetDefaultBucket()
        {
            return await Cluster.Bucket("default");
        }

        public async Task<ICollection> GetDefaultCollection()
        {
            var bucket = await GetDefaultBucket();
            return await bucket.DefaultCollection;
        }

        public void Dispose()
        {
            //Cluster?.Dispose();
        }
    }
}
