using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO;

namespace Couchbase
{
    /// <summary>
    /// Supports Memcached Protocol Add, Set, Replace
    /// </summary>
    public interface IMutationOperation
    {
        IMutationOperation WithExpiry(TimeSpan expiry);

        IMutationOperation WithTimeout(TimeSpan timeout);

        IMutationOperation WithCas(uint cas);

        IMutationOperation WithDurability(PersistTo peristTo, ReplicateTo replicateTo);

        //should return type be the document?
        Task ExecuteAsync();
    }
}
