﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Couchbase.IntegrationTests.Utils
{
    public static class ResourceHelper
    {
        private static readonly Assembly Assembly = typeof(ResourceHelper).GetTypeInfo().Assembly;

        public static T ReadResource<T>(string resourcePath)
        {
            return JsonConvert.DeserializeObject<T>(ReadResource(resourcePath));
        }

        public static string ReadResource(string resourcePath)
        {
            using (var stream = ReadResourceAsStream(resourcePath))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static List<string> ReadResourceAsArray(string resourcePath)
        {
            using (var stream = ReadResourceAsStream(resourcePath))
            {
                if (stream == null)
                {
                    return null;
                }

                var resources = new List<string>();
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        resources.Add(line);
                    }
                }

                return resources;
            }
        }

        public static Stream ReadResourceAsStream(string resourcePath)
        {
            //NOTE: buildOptions.embed for .NET Core ignores the path structure so do a lookup by name
            var index = resourcePath.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            var name = resourcePath.Substring(index, resourcePath.Length-index);
            var resourceName = Assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));

            return Assembly.GetManifestResourceStream(resourceName);
        }
    }
}
