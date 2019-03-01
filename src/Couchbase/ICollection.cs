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

        #region Get

        Task<IGetResult> Get(string id, IEnumerable<string> projections = null, TimeSpan? timeout = null, CancellationToken token = default(CancellationToken));

        Task<IGetResult> Get(string id, Action<GetOptions> optionsAction);

        Task<IGetResult> Get(string id, GetOptions options);

        #endregion

        #region Upsert

        Task<IMutationResult> Upsert<T>(string id, T content,
            TimeSpan? timeout = null,
            TimeSpan expiration = default(TimeSpan),
            ulong cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            DurabilityLevel durabilityLevel = DurabilityLevel.None,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Upsert<T>(string id, T content, Action<UpsertOptions> optionsAction);

        Task<IMutationResult> Upsert<T>(string id, T content, UpsertOptions options);

        #endregion

        #region Insert

        Task<IMutationResult> Insert<T>(string id, T content,
            TimeSpan? timeout = null,
            TimeSpan expiration = default(TimeSpan),
            ulong cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            DurabilityLevel durabilityLevel = DurabilityLevel.None,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Insert<T>(string id, T content, Action<InsertOptions> optionsAction);

        Task<IMutationResult> Insert<T>(string id, T content, InsertOptions options);

        #endregion

        #region Replace

        Task<IMutationResult> Replace<T>(string id, T content, 
            TimeSpan? timeout = null,
            TimeSpan expiration = default(TimeSpan),
            ulong cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            DurabilityLevel durabilityLevel = DurabilityLevel.None,
            CancellationToken token = default(CancellationToken));

        Task<IMutationResult> Replace<T>(string id, T content, Action<ReplaceOptions> optionsAction);

        Task<IMutationResult> Replace<T>(string id, T content, ReplaceOptions options);

        #endregion

        #region Remove

        Task Remove(string id, 
            TimeSpan? timeout = null,
            ulong cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            DurabilityLevel durabilityLevel = DurabilityLevel.None,
            CancellationToken token = default(CancellationToken));

        Task Remove(string id, Action<RemoveOptions> optionsAction);

        Task Remove(string id, RemoveOptions options);

        #endregion

        #region Unlock

        Task Unlock<T>(string id,
            TimeSpan? timeout = null,
            ulong cas = 0,
            CancellationToken token = default(CancellationToken));

        Task Unlock<T>(string id, Action<UnlockOptions> optionsAction);

        Task Unlock<T>(string id, UnlockOptions options);

        #endregion

        #region Touch

        Task Touch(string id, TimeSpan expiration,
            TimeSpan? timeout = null,
            DurabilityLevel durabilityLevel = DurabilityLevel.None,
            CancellationToken token = default(CancellationToken));

        Task Touch(string id, TimeSpan expiration, Action<TouchOptions> optionsAction);

        Task Touch(string id, TimeSpan expiration, TouchOptions options);

        #endregion

        #region LookupIn

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, TimeSpan? timeout = null, CancellationToken token = default(CancellationToken));

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, Action<LookupInOptions> configureOptions);

        Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, LookupInOptions options);

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null, CancellationToken token = default(CancellationToken));

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, Action<LookupInOptions> configureOptions);

        Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, LookupInOptions options);

        #endregion

        #region MutateIn

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false, CancellationToken token = default(CancellationToken));

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, Action<MutateInOptions> configureOptions);

        Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, MutateInOptions options);

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false, CancellationToken token = default(CancellationToken));

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, Action<MutateInOptions> configureOptions);

        Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, MutateInOptions options);

        #endregion
    } 
}
