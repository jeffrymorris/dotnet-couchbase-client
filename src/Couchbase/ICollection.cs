using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public interface ICollection
    {      
        string Cid { get; }

        string Name { get; }

        IBinaryCollection Binary { get; }

        #region GET

        Task<Optional<IGetResult>> Get(string id, TimeSpan timeSpan = new TimeSpan());

        Task<Optional<IGetResult>> Get(string id, GetOptions options);

        Task<Optional<IGetResult>> Get(string id, Action<GetOptions> options);

        #endregion

        #region SET

        Task<IMutationResult> Upsert<T>(string id, T content,
            TimeSpan timeSpan = new TimeSpan(),
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Upsert<T>(string id, T content, UpsertOptions options);

        Task<IMutationResult> Upsert<T>(string id, T content, Action<UpsertOptions> options);

        Task<IMutationResult> Insert<T>(string id, T content, 
            TimeSpan timeSpan = new TimeSpan(),     
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Insert<T>(string id, T content, InsertOptions options);

        Task<IMutationResult> Insert<T>(string id, T content, Action<InsertOptions> options);

        Task<IMutationResult> Replace<T>(string id, T content, 
            TimeSpan timeSpan = new TimeSpan(), 
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero);

        Task<IMutationResult> Replace<T>(string id, T content, ReplaceOptions options);

        Task<IMutationResult> Replace<T>(string id, T content, Action<ReplaceOptions> options);

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

        #region Sub ReadResult

        Task<IMutationResult> MutateIn(string id, MutateInOps ops, MutateInOptions options = default(MutateInOptions));

        Task<IMutationResult> MutateIn(string id, Action<MutateInOps> ops, Action<MutateInOptions> options = null);

        #endregion
        
        Task<ILookupInResult> LookupIn(string id, LookupInOps ops, LookupInOptions options = default(LookupInOptions));

        Task<ILookupInResult> LookupIn(string id, Action<LookupInOps> ops, Action<LookupInOptions> options = default(Action<LookupInOptions>));
    } 
}
