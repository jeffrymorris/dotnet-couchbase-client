using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Couchbase.Core.Configuration.Server;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;
using Couchbase.Core.IO.Transcoders;
using Couchbase.UnitTests.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Couchbase.UnitTests
{
    public class ReadResultTests
    {
        private ITestOutputHelper _output;
        public ReadResultTests(ITestOutputHelper output)
        {
            _output = output;
        }
        public static byte[] PocoBytes =
        {
            129, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0,
            42, 0, 0, 0, 11, 21, 109, 149, 42, 209,
            130, 0, 0, 2, 0, 0, 1, 123, 34,
            110, 97, 109, 101, 34, 58, 34, 98, 111, 98, 34,
            44, 34, 115, 112, 101, 99, 105, 101,
            115, 34, 58, 34, 67, 97, 116, 34, 44,
            34, 97, 103, 101, 34, 58, 53, 125
        };

        public static byte[] LookupBytes =
        {
            129, 208, 0, 0, 0, 0, 0, 204,
            0, 0, 0, 30, 0, 0, 0, 10,
            21, 109, 149, 248, 110, 247, 0, 0,
            0, 0, 0, 0, 0, 7, 91, 49, 44, 50, 44,
            51, 93, 0, 0, 0, 0, 0, 5,
            34, 98, 97, 114, 34, 0, 192, 0, 0, 0, 0
        };

        [Fact]
        public void Test_FullDoc_FullRead()
        {
            var result = new GetResult(PocoBytes,
                new DefaultTranscoder(new DefaultConverter()))
            {
                Id = "id",
                Cas = 0,
                Expiration = TimeSpan.Zero,
                OpCode =  OpCode.Get
            };
            var content = result.ContentAs<dynamic>();
            Assert.Equal("bob", content.name.Value);
        }

        [Fact]
        public void Test_SubDoc_FullRead()
        {
            var readResult = new GetResult(LookupBytes,
                new DefaultTranscoder(new DefaultConverter()),
                new List<OperationSpec>{new OperationSpec
            {
                Path = "bar"
            }, new OperationSpec
                    {
                        Path = "poo"
                    }})
            {
                Id = "id",
                Cas = 0,
                Expiration = TimeSpan.Zero,
                OpCode = OpCode.SubGet
            };
            var content = readResult.ContentAs<dynamic>();
            Assert.Equal("bar", content["foo"]);
        }

        [Fact]
        public void Test_SubDoc_ContentAs_Entire_Doc()
        {

        }

        void Add(JToken token, string name, JObject projection =  null)
        {
            foreach (var child in token.Children())
            {
                if (child is JValue)
                {
                    var value = child as JValue;
                    value.Replace(new JObject(new JProperty(name, projection)));
                    break;
                }
                _output.WriteLine(name);
                Add(child, name, projection);
            }
        }

        [Fact]
        public void Test()
        {
            var path = "attributes".Split(".");
            var json = "{\"type\": \"lima\",\"color\": \"green\",\"chilibean\": false}";
            var projection = JObject.Parse(json);

            var root = new JObject();
            if (path.Length == 1)
            {
                root.Add(new JProperty(path.First(), projection));
            }
            else
            {
                for (var i = 0; i < path.Length; i++)
                {
                    if (root.Last != null)
                    {
                        if (i == path.Length - 1)
                        {
                            Add(root, path[i], projection);
                            continue;
                        }

                        Add(root, path[i]);
                        continue;
                    }

                    root.Add(new JProperty(path[i], null));
                }
            }

            _output.WriteLine(root.ToString());
        }
    }
}
