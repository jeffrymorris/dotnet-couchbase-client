using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface ICollection
    {      
        string Cid { get; }

        string Name { get; }

        #region GET

        Task<IDocument<T>> Get<T>(string id, TimeSpan timeSpan = new TimeSpan());

        Task<IDocument<T>> Get<T>(string id, GetOptions options);

        Task<IDocument<T>> Get<T>(string id, Action<GetOptions> options);

        Task<IDocument<T>> GetAndLock<T>(string id, TimeSpan expiration,
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<T>> GetAndLock<T>(string id, GetAndLockOptions options);

        Task<IDocument<T>> GetAndLock<T>(string id, Action<GetAndLockOptions> options);

        Task<IDocument<T>> GetAndTouch<T>(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan());

        Task<IDocument<T>> GetAndTouch<T>(string id, GetAndTouchOptions options);

        Task<IDocument<T>> GetAndTouch<T>(string id, Action<GetAndTouchOptions> options);

        #endregion

        #region SET

        Task<IDocument<T>> Upsert<T>(IDocument<T> document,
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IDocument<T>> Upsert<T>(IDocument<T> document, UpsertOptions options);

        Task<IDocument<T>> Upsert<T>(IDocument<T> document, Action<UpsertOptions> options);

        Task<IDocument<T>> Insert<T>(IDocument<T> document, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IDocument<T>> Insert<T>(IDocument<T> document, InsertOptions options);

        Task<IDocument<T>> Insert<T>(IDocument<T> document, Action<InsertOptions> options);

        Task<IDocument<T>> Replace<T>(IDocument<T> document, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IDocument<T>> Replace<T>(IDocument<T> document, ReplaceOptions options);

        Task<IDocument<T>> Replace<T>(IDocument<T> document, Action<ReplaceOptions> options);

        #endregion

        #region Remove

        Task<IDocument<T>> Remove<T>(string id, 
            TimeSpan timeSpan = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task Remove<T>(string id, RemoveOptions options);

        Task Remove<T>(string id, Action<RemoveOptions> options);

        #endregion

        #region INCR

        Task Increment(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeout = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<ulong>> Increment(string id, IncrementOptions options);

        Task<IDocument<ulong>> Increment(string id, Action<IncrementOptions> options);

        #endregion

        #region DECR

        Task<IDocument<ulong>> Decrement(string id,
            ulong delta,
            ulong initial,
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<ulong>> Decrement(string id, DecrementOptions options);

        Task<IDocument<ulong>> Decrement(string id, Action<DecrementOptions> options);

        #endregion

        #region Append

        Task<IDocument<string>> Append(string id, string value, 
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<string>> Append(string id, string value, AppendOptions options);

        Task<IDocument<string>> Append(string id, string value, Action<AppendOptions> options);

        Task<IDocument<string>> Append(string id, byte[] value,   
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<byte[]>> Append(string id, byte[] value, AppendOptions options);

        Task<IDocument<byte[]>> Append(string id, byte[] value, Action<AppendOptions> options);

        #endregion

        #region Prepend

        Task<IDocument<byte[]>> Prepend(string id, string value,  
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0);

        Task<IDocument<string>> Prepend(string id, string value, PrependOptions options);

        Task<IDocument<string>> Prepend(string id, string value, Action<PrependOptions> options);

        #endregion

        #region Unlock

        Task Unlock<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan());

        Task Unlock<T>(IDocument<T> document, UnlockOptions options);

        Task Unlock<T>(IDocument<T> document, Action<UnlockOptions> options);

       #endregion

        #region Touch

        Task Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan());

        Task Touch(string id, GetAndTouchOptions options);

        Task Touch(string id, Action<GetAndTouchOptions> options);

        #endregion

        Task<IQueryResponse> Query(string query);
    } 
}
