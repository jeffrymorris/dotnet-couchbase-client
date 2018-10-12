using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface IBucket
    {
        string Name { get; }

        Task BootstrapAsync(Uri uri);

        ICollection this[string name] { get; }

        ICollection GetCollection(string name);

        ICollection GetCollection(string scope, string name);

        IMutationOperation Upsert<T>(IDocument<T> document);

        IMutationOperation Insert<T>(IDocument<T> document);

        IMutationOperation Replace<T>(IDocument<T> document);

        IMutationOperation Remove<T>(IDocument<T> document);

        IFetchOperation<T> Get<T>(string key);

        [Obsolete("Temp solution until server supports getting a manifest.")]
        void LoadManifest(string path);
    }
}
