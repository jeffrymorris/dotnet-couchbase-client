using System;
using System.Collections.Generic;
using Couchbase.UnitTests.Helpers.Wintellect;
using Xunit;
using Xunit.Abstractions;

namespace Couchbase.UnitTests
{
    public class OptionsTests
    {
        private readonly ITestOutputHelper output;

        public OptionsTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test_GetOptions()
        {
            using (var op = new OperationTimer(output))
            {
                for (int i = 0; i < 10000; i++)
                {
                    var option = new MutateOptions().Timeout(new TimeSpan(1, 1, 1)).Upsert("foo", "{bar}")
                        .CreatePath(true).Insert("baz", new {poo = true});
                }
            }
        }

        [Fact]
        public void Test_InsertOptions()
        {
            using (var op = new OperationTimer(output))
            {
                var options = new InsertOptions
                {
                    ReplicateTo = ReplicateTo.One,
                    PersistTo = PersistTo.Four,
                    Cas = long.MaxValue,
                    Timeout = new TimeSpan(0, 0, 1),
                    Expiration = new TimeSpan(0, 0, 3)
                };
            }

            var count = 1000000;

            InsertOptions options1;
            using (var op = new OperationTimer(output))
            {
                while (count-- > 0)
                {
                    options1 = new InsertOptions() {
                        ReplicateTo = ReplicateTo.One,
                        //PersistTo = PersistTo.Four,
                        Cas = long.MaxValue,
                        Timeout = new TimeSpan(0, 0, 1),
                        Expiration = new TimeSpan(0, 0, 3)
                    };

                   // output.WriteLine(count.ToString());
                }
            }
        }
    }
}
