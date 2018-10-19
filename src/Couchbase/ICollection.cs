using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface ICollection
    {      
        string Cid { get; }

        string Name { get; }

        Task<IFetchResult<T>> Get<T>(string id, TimeSpan timeSpan = new TimeSpan());

        Task<IFetchResult<T>> Get<T>(string id, GetOptions options);

        Task<IFetchResult<T>> Get<T>(string id, Action<GetOptions> options);

        Task<IMutationResult> Upsert<T>(IDocument<T> document,
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Upsert<T>(IDocument<T> document, UpsertOptions options);

        Task<IMutationResult> Upsert<T>(IDocument<T> document, Action<UpsertOptions> options);

        Task<IMutationResult> Insert<T>(IDocument<T> document, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Insert<T>(IDocument<T> document, InsertOptions options);

        Task<IMutationResult> Insert<T>(IDocument<T> document, Action<InsertOptions> options);

        Task<IMutationResult> Replace<T>(IDocument<T> document, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Replace<T>(IDocument<T> document, ReplaceOptions options);

        Task<IMutationResult> Replace<T>(IDocument<T> document, Action<ReplaceOptions> options);

        Task<IMutationResult> Remove<T>(string id, 
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Remove<T>(string id, RemoveOptions options);

        Task<IMutationResult> Remove<T>(string id, Action<RemoveOptions> options);

        Task<IMutationResult> Increment(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeout = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Increment(string id, IncrementOptions options);

        Task<IMutationResult> Increment(string id, Action<IncrementOptions> options);

        Task<IMutationResult> Decrement(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Decrement(string id, DecrementOptions options);

        Task<IMutationResult> Decrement(string id, Action<DecrementOptions> options);

        Task<IMutationResult> Append(string id, string value, 
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Append(string id, string value, AppendOptions options);

        Task<IMutationResult> Append(string id, string value, Action<AppendOptions> options);

        Task<IMutationResult> Append(string id, byte[] value,   
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Append(string id, byte[] value, AppendOptions options);

        Task<IMutationResult> Append(string id, byte[] value, Action<AppendOptions> options);

        Task<IMutationResult> Prepend(string id, string value,  
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Prepend(string id, string value, PrependOptions options);

        Task<IMutationResult> Prepend(string id, string value, Action<PrependOptions> options);

        Task<IMutationResult> GetAndLock(string id, TimeSpan expiration,
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> GetAndLock(string id, GetAndLockOptions options);

        Task<IMutationResult> GetAndLock(string id, Action<GetAndLockOptions> options);

        Task<IMutationResult> Unlock(string id, 
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0);

        Task<IMutationResult> Unlock(string id, UnlockOptions options);

        Task<IMutationResult> Unlock(string id, Action<UnlockOptions> options);

        Task<IMutationResult> GetAndTouch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan());

        Task<IMutationResult> GetAndTouch(string id, GetAndTouchOptions options);

        Task<IMutationResult> GetAndTouch(string id, Action<GetAndTouchOptions> options);

        Task<IMutationResult> Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan());

        Task<IMutationResult> Touch(string id, GetAndTouchOptions options);

        Task<IMutationResult> Touch(string id, Action<GetAndTouchOptions> options);

        Task<IQueryResponse> Query(string query);
    } 
}
