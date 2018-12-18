using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public class CouchbaseCollection : ICollection
    {
        internal const string DefaultCollection = "_default";

        public CouchbaseCollection(IBucket bucket, string cid, string name)
        {
            Cid = cid;
            Name = name;
        }

        public string Cid { get; }

        public string Name { get; }

        public Task<Optional<IGetResult>> Get(string id, TimeSpan timeSpan = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<Optional<IGetResult>> Get(string id, GetOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<Optional<IGetResult>> Get(string id, Action<GetOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Upsert<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Upsert<T>(string id, T content, UpsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Upsert<T>(string id, T content, Action<UpsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Insert<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Insert<T>(string id, T content, InsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Insert<T>(string id, T content, Action<InsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Replace<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Replace<T>(string id, T content, ReplaceOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Replace<T>(string id, T content, Action<ReplaceOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id, TimeSpan timeSpan = new TimeSpan(), uint cas = 0, PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id, RemoveOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id, Action<RemoveOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Counter(string id, ulong delta, ulong initial, TimeSpan timeout = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Counter(string id, IncrementOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Counter(string id, Action<IncrementOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Append(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Append(string id, string value, AppendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Append(string id, string value, Action<AppendOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Prepend(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Prepend(string id, string value, PrependOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> Prepend(string id, string value, Action<PrependOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(int id, TimeSpan timeSpan = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(int id, UnlockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(int id, Action<UnlockOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> MutateIn(string id, MutateOptions options = default(MutateOptions))
        {
            throw new NotImplementedException();
        }

        public Task<IStoreResult> MutateIn(string id, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ILookupInResult> LookupIn(string id, LookupInOptions options = default(LookupInOptions))
        {
            throw new NotImplementedException();
        }

        public Task<ILookupInResult> LookupIn(string id, Action<LookupInOptions> options = default(Action<LookupInOptions>))
        {
            throw new NotImplementedException();
        }
    }
}
