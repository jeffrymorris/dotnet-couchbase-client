using System.Threading.Tasks;
using Couchbase.IntegrationTests.Fixtures;
using Xunit;

namespace Couchbase.IntegrationTests
{
    public class SynchronousDurabilityTests : IClassFixture<ClusterFixture>
    {
        private readonly ClusterFixture _fixture;

        public SynchronousDurabilityTests(ClusterFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(DurabilityLevel.None)]
        [InlineData(DurabilityLevel.Majority)]
        [InlineData(DurabilityLevel.MajorityAndPersistActive)]
        [InlineData(DurabilityLevel.PersistToMajority)]
        public async Task Upsert_with_durability(DurabilityLevel durabilityLevel)
        {
            var collection = await _fixture.GetDefaultCollection();

            // Upsert will throw exception if durability is not met
            await collection.Upsert(
                "id",
                new {name = "mike"},
                durabilityLevel: durabilityLevel
            );
        }
    }
}
