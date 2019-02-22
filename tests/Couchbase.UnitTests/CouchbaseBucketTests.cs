using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Couchbase.UnitTests
{
    public class CouchbaseBucketTests
    {
        private const string Ips = "http://10.143.192.101";

        [Fact]
        public async Task GetDefaultCollection_Returns_Default_Collection()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
            {
                UserName = "Administrator",
                Password = "password"
            }.WithServers(Ips).WithBucket("default")).ConfigureAwait(false);

            var bucket =  await cluster.Bucket("default");
            var collection = await bucket.DefaultCollection;

            Assert.NotNull(collection);
            Assert.Equal("_default", collection.Name);
        }

        [Fact]
        public async Task GetScope_Returns_Scope()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var bucket = await cluster.Bucket("default");
            var scope = await bucket.Scope("_default");

            Assert.NotNull(scope);
            Assert.Equal("_default", scope.Name);
        }

        [Fact]
        public async Task GetScope_Throws_ScopeNotFoundException_When_Scope_Not_Found()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var bucket = await cluster.Bucket("default");
            Assert.ThrowsAsync<ScopeNotFoundException>(async ()=> await bucket.Scope("xyz"));
        }

        [Fact]
        public async Task Get_Collection_By_Name()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];
            Assert.NotNull(collection);
        }

        [Fact]
        public async Task Test_Collection_Get()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];

            await collection.Upsert("id", new {foo = "bar"});
            var result = await collection.Get("id");
            var content = result.ContentAs<Person>();
        }

        [Fact]
        public async Task Test_Collection_Get_With_Projections()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];

            await collection.Upsert("id", new {foo = "bar", bar="foo"});
            var result = await collection.Get("id", new List<string>
            {
                "foo",
                "bar"
            });
            var content = result.ContentAs<Person>();
        }
        
        [Fact]
        public async Task Test_Collection_Upsert()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];
            var result =
                await collection.Upsert("id", new Person
                {
                    Age = 16, 
                    FirstName = "Valerie", 
                    LastName = "Morris"
                });
        }

        [Fact]
        public async Task Test_Collection_Insert()
        {
            var key = "Test_Collection_Insert";
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];

            try
            {
                await collection.Remove(key);
            }
            catch (Exception)
            {
                //ignore
            }

            var result =
                await collection.Insert(key, new Person
                {
                    Age = 16, 
                    FirstName = "Valerie", 
                    LastName = "Morris"
                });
        }

        [Fact]
        public async Task Test_Collection_Replace()
        {
            var id = "Test_Collection_Replace";
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = cluster.Bucket("default").Result.Scope("_default").Result["_default"];

            try
            {
                await collection.Remove(id);
            }
            catch (Exception e)
            {

            }

            var insert = await collection.Insert(id, new Person
            {
                Age = 18,
                FirstName = "Valerie",
                LastName = "Morris"
            });

            var replace = await collection.Replace(id, new Person
            {
                Age = 21, 
                FirstName = "Valerie", 
                LastName = "Morris"
            });

            var get = await collection.Get(id);
            Assert.Equal(21, get.ContentAs<Person>().Age);
        }

      //  [Fact]
        public async Task Test_Collection_Upserts_Lots()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];

            var items = new List<Task<IMutationResult>>();
            for (int i = 0; i < 1000000; i++)
            {
                items.Add(collection.Upsert("Test_Collection_Insert_" + i, new Person
                {
                    Age = 16,
                    FirstName = "Valerie",
                    LastName = "Morris"
                }));
            }

           await Task.WhenAll(items);
        }

        [Fact]
        public async Task Test_Collection_Timeout()
        {
            var cluster = new Cluster();
            await cluster.Initialize(new Configuration
                    {
                        UserName = "Administrator",
                        Password = "password"
                    }.WithServers(Ips)
                    .WithBucket("default"))
                .ConfigureAwait(false);

            var collection = (await (await cluster.Bucket("default")).Scope("_default"))["_default"];
            var result = await collection.Get("id", timeout: new TimeSpan(0, 0, 1, 0, 1));
        }

        public class Person
        {
            public int Age { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }
    }
}
