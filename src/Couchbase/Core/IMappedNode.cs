using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase.Core
{
    public interface IMappedNode
    {
        IServer LocatePrimary();

        uint Rev { get; }
    }
}
