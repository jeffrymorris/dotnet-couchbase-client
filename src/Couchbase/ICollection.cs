using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Couchbase.Services.Query;

namespace Couchbase
{
    public interface ICollection
    {      
        string Cid { get; }

        string Name { get; }

        IMutationOperation Upsert<T>(IDocument<T> document);

        IMutationOperation Insert<T>(IDocument<T> document);

        IMutationOperation Replace<T>(IDocument<T> document);

        IMutationOperation Remove<T>(IDocument<T> document);

        IFetchOperation<T> Get<T>(string key);

        IQueryResponse Query(string query);
    } 
}
