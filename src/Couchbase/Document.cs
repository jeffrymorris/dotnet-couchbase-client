using System;
using Couchbase.Core;

namespace Couchbase
{
    public class Document<T> : IDocument<T>
    {
        public string Id { get; set; }

        public T Content { get; set; }

        public TimeSpan Expiry { get; set; }

        public long Cas { get; internal set; }

        public MutationToken MutationToken { get; internal set; }
    }
}