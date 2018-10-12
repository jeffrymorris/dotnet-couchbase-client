using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase
{
    public interface IConfiguration
    {
        IConfiguration WithServers(params string[] ips);

        IConfiguration WithBucket(params string[] bucketNames);

        IConfiguration WithCredentials(string username, string password);

        IEnumerable<Uri> Servers { get; }

        IEnumerable<string> Buckets { get; }

        string UserName { get; set; }

        string Password { get; set; }
    }
}
