using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Couchbase.UnitTests
{
    public class CouchbaseCollectionsTests
    {
        [Fact]
        public async Task Scenario_A()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            // 1) fetch a full readResult that is a json readResult
            var result = await collection.Get("key1",
                options => { options.Timeout = new TimeSpan(0, 0, 1); });

            // 2) Make a modification to the content
            var person = result.ContentAs<dynamic>();
            person.age = 45;
            person.arms = 1;

            // 3) replace the readResult on the server
            var mutateResults = await collection.Replace("key1", (object) person,
                options => { options.Timeout = new TimeSpan(); });

            Assert.True(mutateResults.Cas > 0);
        }

        [Fact]
        public async Task Scenario_B()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            //1) fetch a readResult fragment which is a json array with elements
            var result = await collection.Get("key1",
                options => {
                    options.CreatePath = true;
                    options.Project("legs");
                });

            // 2) make modifications to the content
            var items = result.ContentAs<List<int>>();
            items.Add(13);
            items.Add(42);

            //3) replace the fragment in the original readResult - NOTE: ArrayAppend on the server is better
            var mutateResults = await collection.MutateIn("key1",
                ops =>
                {
                    ops.Replace("legs", items);
                });

            Assert.True(mutateResults.Cas > 0);
        }

        [Fact]
        public async Task Scenario_C()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            //old skewl observe-based/mutation token durability
            await collection.Remove("key1", options =>
            {
                options.PersistTo = PersistTo.One;
                options.ReplicateTo = ReplicateTo.Two;
            });

            //new skewl 'asynchronous' durability
            await collection.Remove("key2", options =>
            {
                options.DurabilityLevel = DurabilityLevel.Majority;
            });
        }

        [Fact]
        public async Task Scenario_D()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            //Note this is the same as A since we pass the doc in with the CAS

            do
            {
                // 1) fetch a full readResult that is a json readResult
                var result = await collection.Get("key1",
                    options => { options.Timeout = new TimeSpan(0, 0, 1); });

                // 2) Make a modification to the content
                var person = result.ContentAs<dynamic>();
                person.age = 45;
                person.arms = new List<int> {1};

                try
                {

                    // 3) replace the readResult on the server
                    var mutateResults = await collection.Replace("key1", (object) person,
                        options =>
                        {
                            options.Timeout = new TimeSpan(0, 0, 1);
                        });
                }
                catch (KeyValueException e)
                {
                }
            } while (true);
        }

        /// <summary>
        /// "Entity" type object
        /// </summary>
        public class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public List<int> Arms { get; set; }
        }

        [Fact]
        public async Task Scenario_E()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            // 1) Fetch a full ReadResult and marshal it into a language entity rather than a generic json type
            var result = await collection.Get("key1",
                options => { options.Timeout = new TimeSpan(0, 0, 1); });

            // 2) Make a modification to the content
            var person = result.ContentAs<Person>();
            person.Age = 45;
            person.Arms = new List<int> {1};

            // 3) replace the readResult on the server
            var mutateResults = await collection.Replace("key1", person, 
                options => { options.Timeout  = new TimeSpan(0, 0, 1); });

            Assert.True(mutateResults.Cas > 0);
        }

        [Fact]
        public async Task Scenario_F()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            var result = await collection.Get("key2",
                options =>
                {
                    options.WithTimeout(new TimeSpan(0, 0, 0, 5));
                    options.WithCreatePath(false);
                    options.Project("age", "arms", "poo", "bar");
              });

            // 2) Make a modification to the content
            var person = result.ContentAs<Person>();
            person.Age = 45;
            person.Arms = new List<int> {1};

            // 3) replace the readResult on the server
            var mutateResults = await collection.MutateIn("key1",
                ops =>
                {
                    ops.Replace("age", person.Age);
                    ops.Replace("arms", person.Arms);
                },
                options =>
                {
                    options.Timeout(new TimeSpan(0, 0, 1));
                });

            Assert.True(mutateResults.Cas > 0);
        }

        [Fact]
        public async Task Scenario_F_2()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            var result = await collection.Get("key2",
                new GetOptions().
                    WithTimeout(new TimeSpan(0, 0, 0, 5)).
                    Project("age", "arms", "poo", "bar"));

            // 2) Make a modification to the content
            var person = result.ContentAs<Person>();
            person.Age = 45;
            person.Arms = new List<int> {1};

            // 3) replace the readResult on the server
            var mutateResults = await collection.MutateIn("key1", ops =>
                {
                    ops.Replace("age", person.Age);
                    ops.Replace("arms", person.Arms);
                },
                options => { options.Timeout(new TimeSpan(0, 0, 1)); });

            Assert.True(mutateResults.Cas > 0);
        }

        [Fact]
        public async Task MutateIn_Test()
        {
            var mockBucket = new Mock<IBucket>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default");

            var result = await collection.MutateIn("L33T", ops =>
            {
                ops.Insert("thepath2", SubdocPathFlags.CreatePath | SubdocPathFlags.Xattr);
                ops.Insert("thepath", 22, true, true);
                ops.Upsert("anotherpath", "eww");
                ops.ArrayAppend("anarray", new object[] {"a", 3, "c"}, true);
            }, options => options.Timeout(milliseconds:20));
        }
    }
}
