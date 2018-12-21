using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface IBinaryCollection
    {
        #region GET

        Task<Optional<IGetResult>> Get(string id, TimeSpan timeSpan = new TimeSpan());

        Task<Optional<IGetResult>> Get(string id, GetOptions options);

        Task<Optional<IGetResult>> Get(string id, Action<GetOptions> options);

        #endregion

        #region SET

        Task<IStoreResult> Upsert<T>(string id, T content,
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IStoreResult> Upsert<T>(string id, T content, UpsertOptions options);

        Task<IStoreResult> Upsert<T>(string id, T content, Action<UpsertOptions> options);

        Task<IStoreResult> Insert<T>(string id, T content, 
            TimeSpan timeSpan = new TimeSpan(),     
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IStoreResult> Insert<T>(string id, T content, InsertOptions options);

        Task<IStoreResult> Insert<T>(string id, T content, Action<InsertOptions> options);

        Task<IStoreResult> Replace<T>(string id, T content, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IStoreResult> Replace<T>(string id, T content, ReplaceOptions options);

        Task<IStoreResult> Replace<T>(string id, T content, Action<ReplaceOptions> options);

        #endregion

        #region Remove

        Task Remove(string id, 
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task Remove(string id, RemoveOptions options);

        Task Remove(string id, Action<RemoveOptions> options);

        #endregion

        #region Append

        Task<IStoreResult> Append(string id, string value, 
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IStoreResult> Append(string id, string value, AppendOptions options);

        Task<IStoreResult> Append(string id, string value, Action<AppendOptions> options);

        #endregion

        #region Prepend

        Task<IStoreResult> Prepend(string id, string value,  
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IStoreResult> Prepend(string id, string value, PrependOptions options);

        Task<IStoreResult> Prepend(string id, string value, Action<PrependOptions> options);

        #endregion

        #region INCR & DECR

        Task<IStoreResult> Increment(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeout = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IStoreResult> Increment(string id, IncrementOptions options);

        Task<IStoreResult> Increment(string id, Action<IncrementOptions> options);

        Task<IStoreResult> Decrement(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeout = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IStoreResult> Decrement(string id, IncrementOptions options);

        Task<IStoreResult> Decrement(string id, Action<IncrementOptions> options);

        #endregion

        #region Unlock

        Task Unlock<T>(int id, TimeSpan timeSpan = new TimeSpan());

        Task Unlock<T>(int id, UnlockOptions options);

        Task Unlock<T>(int id, Action<UnlockOptions> options);

        #endregion

        #region Touch

        Task Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan());

        Task Touch(string id, GetAndTouchOptions options);

        Task Touch(string id, Action<GetAndTouchOptions> options);

        #endregion
    }
}
