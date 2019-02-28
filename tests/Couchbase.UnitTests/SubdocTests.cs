using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Couchbase.UnitTests
{
    public class SubdocTests : IClassFixture<ClusterFixture>
    {
        private const string DocumentKey = "document-key";
        private readonly ClusterFixture _fixture;

        public SubdocTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Can_perform_lookup_in()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(DocumentKey, new {foo = "bar", bar = "foo"});

            var result = await collection.LookupIn(DocumentKey, ops =>
            {
                ops.Get("foo");
                ops.Get("bar");
            });

            Assert.Equal("bar", result.ContentAs<string>(0));
            Assert.Equal("foo", result.ContentAs<string>(1));
        }

        [Fact]
        public async Task Can_do_lookup_in_with_array()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(DocumentKey, new {foo = "bar", bar = "foo"});

            var result = await collection.LookupIn(DocumentKey, new []
            {
                LookupInSpec.Get("foo"),
                LookupInSpec.Get("bar")
            });

            Assert.Equal("bar", result.ContentAs<string>(0));
            Assert.Equal("foo", result.ContentAs<string>(1));
        }

        [Fact]
        public async Task Can_perform_mutate_in()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(DocumentKey,  new {foo = "bar", bar = "foo"});

            await collection.MutateIn(DocumentKey, ops =>
            {
                ops.Upsert("name", "mike");
                ops.Replace("bar", "bar");
            });

            var getResult = await collection.Get(DocumentKey);
            var content = getResult.ContentAs<string>();

            var expected = new
            {
                foo = "bar",
                bar = "bar",
                name = "mike"
            };
            Assert.Equal(JsonConvert.SerializeObject(expected), content);
        }

        [Fact]
        public async Task Can_perform_mutate_in_with_array()
        {
            var collection = await _fixture.GetDefaultCollection();
            await collection.Upsert(DocumentKey, new {foo = "bar", bar = "foo"});

            await collection.MutateIn(DocumentKey, new[]
            {
                MutateInSpec.Upsert("name", "mike"),
                MutateInSpec.Replace("bar", "bar")
            });

            var getResult = await collection.Get(DocumentKey);
            var content = getResult.ContentAs<string>();

            var expected = new
            {
                foo = "bar",
                bar = "bar",
                name = "mike"
            };
            Assert.Equal(JsonConvert.SerializeObject(expected), content);
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
                    .WithServers("couchbase://127.0.0.1")
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
