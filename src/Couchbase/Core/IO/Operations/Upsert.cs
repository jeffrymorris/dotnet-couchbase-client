using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.Core.IO.Operations
{
    public class Upsert : IMutationOperation
    {
        public IMutationOperation WithExpiry(TimeSpan expiry)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithTimeout(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithCas(uint cas)
        {
            throw new NotImplementedException();
        }

        public IMutationOperation WithDurability(PersistTo peristTo, ReplicateTo replicateTo)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
