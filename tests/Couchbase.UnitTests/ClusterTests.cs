using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NUnit.Framework;


namespace Couchbase.UnitTests
{
    [TestFixture]
    public class ClusterTests
    {
        [Test]
        public async Task Test_ClusterCreation()
        {
            //cluster n' stuff
            var cluster = new Cluster();
            cluster.ConnectAsync(new Configuration());

            //collection from bucket
            var collection = cluster.GetBucket("boo").GetCollection("key");

            //configure an operation on the collection
            var set = collection.Upsert(new Document<dynamic>())   
                .WithDurability(PersistTo.One, ReplicateTo.One)
                .WithExpiry(TimeSpan.FromHours(1))
                .WithTimeout(TimeSpan.FromMinutes(1))
                .WithCas(0);

            await set.ExecuteAsync();
            
            var get = collection.Get<dynamic>("thekey")
                .WithDurability(PersistTo.One, ReplicateTo.One) 
                .WithTimeout(TimeSpan.FromMinutes(1));

            var result = await get.ExecuteAsync();

            var query = collection.Query("SELECT * FROM `foo`;");

            var count = 0;
            while (count++<1000000)
            {
                cluster.GetBucket("boo").GetCollection("key");
            }
        }
    }
}
