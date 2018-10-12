using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Core.Configuration.Server
{
    public class Ports
    {
        public int direct { get; set; }
    }

    public class Node
    {
        public string couchApiBase { get; set; }
        public string hostname { get; set; }
        public Ports ports { get; set; }
    }

    public class Services
    {
        public int mgmt { get; set; }
        public int mgmtSSL { get; set; }
        public int indexAdmin { get; set; }
        public int indexScan { get; set; }
        public int indexHttp { get; set; }
        public int indexStreamInit { get; set; }
        public int indexStreamCatchup { get; set; }
        public int indexStreamMaint { get; set; }
        public int indexHttps { get; set; }
        public int kv { get; set; }
        public int kvSSL { get; set; }
        public int capi { get; set; }
        public int capiSSL { get; set; }
        public int projector { get; set; }
        public int n1ql { get; set; }
        public int n1qlSSL { get; set; }
    }

    public class NodesExt
    {
        public Services services { get; set; }
        public bool thisNode { get; set; }
    }

    public class Ddocs
    {
        public string uri { get; set; }
    }

    public class VBucketServerMap
    {
        public string hashAlgorithm { get; set; }
        public int numReplicas { get; set; }
        public List<string> serverList { get; set; }
        public List<List<int>> vBucketMap { get; set; }
    }

    //Root object
    public class ClusterMap
    {
        public int rev { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public string streamingUri { get; set; }
        public List<Node> nodes { get; set; }
        public List<NodesExt> nodesExt { get; set; }
        public string nodeLocator { get; set; }
        public string uuid { get; set; }
        public Ddocs ddocs { get; set; }
        public VBucketServerMap vBucketServerMap { get; set; }
        public string bucketCapabilitiesVer { get; set; }
        public List<string> bucketCapabilities { get; set; }
    }
}
