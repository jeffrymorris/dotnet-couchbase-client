using System;
using System.Collections.Generic;
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

        Task<IGetResult> Get(string id, IEnumerable<string> projections = null, TimeSpan? timeout = null, CancellationToken token = default(CancellationToken));

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

        #region LookupIn

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, TimeSpan? timeout = null);

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, Action<LookupInOptions> configureOptions);

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, LookupInOptions options);

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null);

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, Action<LookupInOptions> configureOptions);

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, LookupInOptions options);

        #endregion

        #region MutateIn

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false);

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, Action<MutateInOptions> configureOptions);

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, MutateInOptions options);

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false);

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, Action<MutateInOptions> configureOptions);

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, MutateInOptions options);

        #endregion
    } 
}
