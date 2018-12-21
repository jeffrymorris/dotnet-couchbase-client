using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;

namespace Couchbase.UnitTests
{
    public class BinaryCollectionTests
    {
        [Fact]
        public void Get_BinaryCollection()
        {
            var mockBucket = new Mock<IBucket>();
            var mockBinaryCollection = new Mock<IBinaryCollection>();
            var collection = new CouchbaseCollection(mockBucket.Object, "0x0", "_default", mockBinaryCollection.Object);

            var binaryCollection = collection.Binary;
            var result = binaryCollection.Get("id");

            //or

            var result1 = collection.Binary.Get("id");
        }
    }
}
