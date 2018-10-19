using System;
using System.Collections.Generic;
using System.Text;
using Couchbase.Core;

namespace Couchbase
{
    public interface IDocument<T>
    {
        string Id { get; set; }

        T Content { get; set; }

        TimeSpan Expiry { get; set; }

        long Cas { get; }

        MutationToken MutationToken { get; }
    }
}
