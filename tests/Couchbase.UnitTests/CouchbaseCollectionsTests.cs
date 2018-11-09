using System;
using System.Threading.Tasks;
using Xunit;

namespace Couchbase.UnitTests
{
    public class CouchbaseCollectionsTests
    {
        [Fact]
        public async Task Test()
        {
            //create a cluster and connect to it
            var cluster = new Cluster();
            await cluster.ConnectAsync(new Configuration()).ConfigureAwait(false);

            //get the bucket and people collection with scope name
            var bucket = cluster.GetBucket("default");
            var people = bucket.GetCollection("myscope", "people");

            //fetch the doc header and explicitly do not include the body
            var person = await people.Get<dynamic>("id", options =>
            {
                options.Timeout = new TimeSpan(0, 0, 30);
                options.IncludeBody = false;
            }).ConfigureAwait(false);

            //update the doc on the server
            person.Insert("hair", "brown");
            person.Insert("eyes", "green");

            //update it..
            person.Upsert("eyes", "blue");

            //fetch just the eye color
            var eyeColor = person.Get<string>("eyes");
            Console.WriteLine("My eyes are {eyeColor}", eyeColor);

            //go fetch the doc body - if IncludeBody is false this will do a GET fetch or if true pull local cached
            var json = person.Body;
            Console.WriteLine(json);

            //write a new extended attribute
            person.SetAttribute("dateInactive", new DateTime(2018, 10, 23));

            //get the new extended attribute
            var dateInactive = person.GetAttribute<DateTime>("dateInactive");
            Console.WriteLine("This account de-activated {dateInactive}", dateInactive);

            //get the extended virtual attribute expires
            var expires = person.GetVirtualAttribute<DateTime>("exptime");
            Console.WriteLine("This record was added {expires}", expires);
        }
    }
}
