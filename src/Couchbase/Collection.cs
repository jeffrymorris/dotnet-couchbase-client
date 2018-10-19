using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase
{
    public class Collection : ICollection
    {
        public Collection(IBucket bucket, string cid, string name)
        {
            Bucket = bucket;
            Cid = cid;
            Name = name;
        }

        protected IBucket Bucket { get; }

        public string Cid { get; }

        public string Name { get; }

        public Task<IFetchResult<T>> Get<T>(string id, TimeSpan timeSpan = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<IFetchResult<T>> Get<T>(string id, GetOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IFetchResult<T>> Get<T>(string id, Action<GetOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(IDocument<T> document, UpsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(IDocument<T> document, Action<UpsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(IDocument<T> document, InsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(IDocument<T> document, Action<InsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(IDocument<T> document, ReplaceOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(IDocument<T> document, Action<ReplaceOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Remove<T>(string id, TimeSpan timeSpan = new TimeSpan(), uint cas = 0, PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Remove<T>(string id, RemoveOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Remove<T>(string id, Action<RemoveOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Increment(string id, ulong delta, ulong initial, TimeSpan timeout = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Increment(string id, IncrementOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Increment(string id, Action<IncrementOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Decrement(string id, ulong delta, ulong initial, TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Decrement(string id, DecrementOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Decrement(string id, Action<DecrementOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, string value, AppendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, string value, Action<AppendOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, byte[] value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, byte[] value, AppendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Append(string id, byte[] value, Action<AppendOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Prepend(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Prepend(string id, string value, PrependOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Prepend(string id, string value, Action<PrependOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndLock(string id, TimeSpan expiration, TimeSpan timeSpan = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndLock(string id, GetAndLockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndLock(string id, Action<GetAndLockOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Unlock(string id, TimeSpan timeSpan = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Unlock(string id, UnlockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Unlock(string id, Action<UnlockOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndTouch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndTouch(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> GetAndTouch(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Touch(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Touch(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResponse> Query(string query)
        {
            throw new NotImplementedException();
        }
    }
}
