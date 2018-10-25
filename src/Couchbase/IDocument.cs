using System;
using System.Collections.Generic;
using System.Text;
using Couchbase.Core;

namespace Couchbase
{
    public interface IDocument
    {
        string Id { get; set; }

        TimeSpan Expiry { get; set; }

        long Cas { get; }

        MutationToken MutationToken { get; }
    }
}
