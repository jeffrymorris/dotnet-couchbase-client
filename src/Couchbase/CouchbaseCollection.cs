using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public class CouchbaseCollection : ICollection
    {
        internal const string DefaultCollection = "_default";

        public CouchbaseCollection(IBucket bucket, string cid, string name, IBinaryCollection binaryCollection =null)
        {
            Cid = cid;
            Name = name;
            Binary = binaryCollection;
        }

        public string Cid { get; }

        public string Name { get; }

        public IBinaryCollection Binary { get; }

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

        public Task<IMutationResult> Upsert<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(string id, T content, UpsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(string id, T content, Action<UpsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(string id, T content, InsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(string id, T content, Action<InsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(string id, T content, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(string id, T content, ReplaceOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(string id, T content, Action<ReplaceOptions> options)
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

        public Task<IMutationResult> MutateIn(string id, MutateInOps ops, MutateInOptions options = default(MutateInOptions))
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> MutateIn(string id, Action<MutateInOps> ops, Action<MutateInOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public Task<ILookupInResult> LookupIn(string id, LookupInOps ops, LookupInOptions options = default(LookupInOptions))
        {
            throw new NotImplementedException();
        }

        public Task<ILookupInResult> LookupIn(string id, Action<LookupInOps> ops, Action<LookupInOptions> options = default(Action<LookupInOptions>))
        {
            throw new NotImplementedException();
        }
    }
}
