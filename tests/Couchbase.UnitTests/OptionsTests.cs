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
        public void Test_InsertOptions()
        {
            using (var op = new OperationTimer(output))
            {
                var options = new InsertOptions().
                    ReplicateTo(ReplicateTo.One).
                    PersistTo(PersistTo.Four).
                    Cas(long.MaxValue).
                    Timeout(new TimeSpan(0, 0, 1)).
                    Expiration(new TimeSpan(0, 0, 3));
            }

            var count = 1000000;

            InsertOptions options1;
            using (var op = new OperationTimer(output))
            {
                while (count-- > 0)
                {
                    options1 = new InsertOptions().
                        ReplicateTo(ReplicateTo.One).
                        /// PersistTo(PersistTo.Four).
                        Cas(count).
                        Timeout(new TimeSpan(0, 0, 1)).
                        Expiration(new TimeSpan(0, 0, 3));

                   // output.WriteLine(count.ToString());
                }
            }
        }

        [Fact]
        public void Test_UpsertOptions()
        {
            var count = 1000000;

            InsertOptions options1;
            using (var op = new OperationTimer(output))
            {
                while (count-- > 0)
                {
                    options1 = new InsertOptions().
                        ReplicateTo(ReplicateTo.One).
                        /// PersistTo(PersistTo.Four).
                        Cas(count).
                        Timeout(new TimeSpan(0, 0, 1)).
                        Expiration(new TimeSpan(0, 0, 3));

                    // output.WriteLine(count.ToString());
                }
            }
        }
    }
}
