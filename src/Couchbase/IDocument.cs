using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase
{
    public interface IDocument<T>
    {
        string Key { get; set; }

        T Content { get; set; }
    }
}
