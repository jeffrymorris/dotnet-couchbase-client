using System;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public interface ICollection
    {      
        uint Cid { get; }

        string Name { get; }

        IBinaryCollection Binary { get; }

        #region GET

        Task<IGetResult> Get(string id,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken));

        Task<IGetResult> Get(string id, GetOptions options);

        Task<IGetResult> Get(string id, Action<GetOptions> options);

        #endregion

        #region SET

        Task<IMutationResult> Upsert<T>(string id, T content,
            TimeSpan? timeout = null,
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Upsert<T>(string id, T content, UpsertOptions options);

        Task<IMutationResult> Upsert<T>(string id, T content, Action<UpsertOptions> options);

        Task<IMutationResult> Insert<T>(string id, T content,
            TimeSpan? timeout = null,
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Insert<T>(string id, T content, InsertOptions options);

        Task<IMutationResult> Insert<T>(string id, T content, Action<InsertOptions> options);

        Task<IMutationResult> Replace<T>(string id, T content, 
            TimeSpan? timeout = null,
            TimeSpan expiration = new TimeSpan(), 
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Replace<T>(string id, T content, ReplaceOptions options);

        Task<IMutationResult> Replace<T>(string id, T content, Action<ReplaceOptions> options);

        #endregion

        #region Remove

        Task Remove(string id, 
            TimeSpan? timeout = null,
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            CancellationToken token = default(CancellationToken));

        Task Remove(string id, RemoveOptions options);

        Task Remove(string id, Action<RemoveOptions> options);

        #endregion

        #region Unlock

        Task Unlock<T>(string id,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken));

        Task Unlock<T>(string id, UnlockOptions options);

        Task Unlock<T>(string id, Action<UnlockOptions> options);

        #endregion

        #region Touch

        Task Touch(string id, TimeSpan expiration,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken));

        Task Touch(string id, GetAndTouchOptions options);

        Task Touch(string id, Action<GetAndTouchOptions> options);

        #endregion

        Task<IMutationResult> MutateIn(string id, OperationSpec[] specs, Action<MutateInOptions> options = default(Action<MutateInOptions>));

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpec> ops, Action<MutateInOptions> options = default(Action<MutateInOptions>));

        Task<ILookupInResult> LookupIn(string id, OperationSpec[] specs, Action<LookupInOptions> options = default(Action<LookupInOptions>));

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpec> ops, Action<LookupInOptions> options = default(Action<LookupInOptions>));
    } 
}
