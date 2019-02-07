using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Couchbase.Services.Query.Couchbase.N1QL;
using Xunit;


namespace Couchbase.UnitTests
{
    public class ClusterTests
    {
        [Fact]
        public async Task Authenticate_Does_Not_Throw_Exception()
        {

            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task Authenticate_Throws_ArgumentNullException_When_Crendentials_Not_Provided()
        {

            var cluster = new Cluster();
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await cluster.Initialize(new Configuration()).ConfigureAwait(false));
        }

        [Fact]
        public async Task GetBucket_Returns_Bucket()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var bucket = cluster.Bucket("default");
            Assert.NotNull(bucket);
        }

        [Fact]
        public async Task Test_Query()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }).ConfigureAwait(false);

            var result = cluster.Query<dynamic>("SELECT x.* FROM `default` WHERE x.Type=&0",
                parameter => parameter.Add("poo"), 
                options => options.Encoding(Encoding.Utf8));
        }

        [Fact]
        public async Task Test_Query2()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }.WithServers(" http://10.143.192.101:8091").WithBucket("default")).ConfigureAwait(false);

            var result = await cluster.Query<dynamic>("SELECT * FROM `default` WHERE type=$name;",
                parameter =>
            {
                parameter.Add("name", "poo");
            }).ConfigureAwait(false);

            
            foreach (var o in result)
            {
                
            }
            result.Dispose();
        }
    }
}
