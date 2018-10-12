using System.Collections.Generic;

namespace Couchbase.Services.Collections
{
    public class Collection
    {
        public string name { get; set; }
        public string uid { get; set; }
    }

    public class Scope
    {
        public string name { get; set; }
        public string uid { get; set; }
        public List<Collection> collections { get; set; }
    }

    public class Manifest
    {
        public string uid { get; set; }
        public List<Scope> scopes { get; set; }
    }
}
