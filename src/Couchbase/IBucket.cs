    using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface IBucket
    {
        string Name { get; }

        Task BootstrapAsync(Uri uri);

        IScope this[string name] { get; }

        ICollection GetDefaultCollection();

        IScope GetScope(string name);

        [Obsolete("Temp solution until server supports getting a manifest.")]
        void LoadManifest(string path);
    }
}
