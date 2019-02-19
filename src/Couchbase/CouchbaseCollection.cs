using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Core.IO.Operations.Legacy.SubDocument;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public class CouchbaseCollection : ICollection
    {
        internal const string DefaultCollection = "_default";
        private readonly IBucket _bucket;
        private static readonly TimeSpan DefaultTimeout = new TimeSpan(0,0,0,0,2500);//temp

        public CouchbaseCollection(IBucket bucket, string cid, string name, IBinaryCollection binaryCollection =null)
        {
            Cid = Convert.ToUInt32(cid);
            Name = name;
            Binary = binaryCollection;
            _bucket = bucket;
        }

        public uint Cid { get; }

        public string Name { get; }

        public IBinaryCollection Binary { get; }

        public async Task ExecuteOp(IOperation op, 
            TaskCompletionSource<byte[]> tcs, 
            CancellationToken token = default(CancellationToken),
            TimeSpan? timeout = null)
        {
            CancellationTokenSource cts = null;
            if (token == CancellationToken.None)
            {
                cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(timeout ?? DefaultTimeout);
                token = cts.Token;
            }

            using (token.Register(() =>
            {
                if (tcs.Task.Status != TaskStatus.RanToCompletion)
                {
                    tcs.SetCanceled();
                }
            }, useSynchronizationContext: false))
            {
                await ((IBucketSender) _bucket).Send(op, tcs).ConfigureAwait(false);
                var bytes = await tcs.Task.ConfigureAwait(false);
                await op.ReadAsync(bytes).ConfigureAwait(false);

                //clean up the token if we used a default token
                cts?.Dispose();
            }
        }

        public async Task<IGetResult> Get(string id,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var getOp = new Get<object>
            {
                Key = id,
                Cid = Cid,
                VBucketId = 752,
                Completed = s =>
                {
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());                 
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            CancellationTokenSource cts = null;
            if (token == CancellationToken.None)
            {
                cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(timeout ?? DefaultTimeout);
                token = cts.Token;
            }
            using (token.Register(() =>
            {
                if (tcs.Task.Status != TaskStatus.RanToCompletion)
                {
                    tcs.SetCanceled();
                }
            }, useSynchronizationContext: false))
            {
                await ((IBucketSender) _bucket).Send(getOp, tcs).ConfigureAwait(false);           
                var bytes = await tcs.Task.ConfigureAwait(false);
                await getOp.ReadAsync(bytes).ConfigureAwait(false);

                //clean up the token if we used a default token
                cts?.Dispose();
                
                return new GetResult(bytes, getOp.Key, getOp.Cas, null, true);
            }
        }

        public Task<IGetResult> Get(string id, GetOptions options)
        {
            return Get(id, options.Timeout);
        }

        public Task<IGetResult> Get(string id, Action<GetOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task<IMutationResult> Upsert<T>(string id, T content, TimeSpan? timeout = null, TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero, CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var upsertOp = new Set<T>
            {
                Key = id,
                Content = content,
                Cas = cas,
                Cid = Cid,
                VBucketId = 752,
                Expires = expiration.ToTtl(),
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(upsertOp, tcs, token, timeout).ConfigureAwait(false);
            return new MutationResult(upsertOp.Cas, null, upsertOp.MutationToken);
        }

        public Task<IMutationResult> Upsert<T>(string id, T content, UpsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Upsert<T>(string id, T content, Action<UpsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task<IMutationResult> Insert<T>(string id, T content, TimeSpan? timeout = null, TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero, CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var insertOp = new Add<T>
            {
                Key = id,
                Content = content,
                Cas = cas,
                Cid = Cid,
                Expires = expiration.ToTtl(),
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };
  
            await ExecuteOp(insertOp, tcs, token, timeout).ConfigureAwait(false);
            return new MutationResult(insertOp.Cas, null, insertOp.MutationToken);
        }

        public Task<IMutationResult> Insert<T>(string id, T content, InsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Insert<T>(string id, T content, Action<InsertOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task<IMutationResult> Replace<T>(string id,
            T content,
            TimeSpan? timeout = null,
            TimeSpan expiration = new TimeSpan(),
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero, 
            CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var replaceOp = new Replace<T>
            {
                Key = id,
                Content = content,
                Cas = cas,
                Cid = Cid,
                Expires = expiration.ToTtl(),
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(replaceOp, tcs, token, timeout).ConfigureAwait(false);
            return new MutationResult(replaceOp.Cas, null, replaceOp.MutationToken);
        }

        public Task<IMutationResult> Replace<T>(string id, T content, ReplaceOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IMutationResult> Replace<T>(string id, T content, Action<ReplaceOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task Remove(string id,
            TimeSpan? timeout = null,
            uint cas = 0,
            PersistTo persistTo = PersistTo.Zero,
            ReplicateTo replicateTo = ReplicateTo.Zero,
            CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var removeOp = new Delete
            {
                Key = id,
                Cas = cas,
                Cid = Cid,
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(removeOp, tcs, token, timeout).ConfigureAwait(false);
        }

        public Task Remove(string id, RemoveOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id, Action<RemoveOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task Unlock<T>(string id,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var unlockOp = new Unlock
            {
                Key = id,
                Cid = Cid,
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(unlockOp, tcs, token, timeout).ConfigureAwait(false);
        }

        public Task Unlock<T>(string id, UnlockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(string id, Action<UnlockOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task Touch(string id,
            TimeSpan expiration,
            TimeSpan? timeout = null,
            CancellationToken token = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();
            var touchOp = new Touch
            {
                Key = id,
                Cid = Cid,
                Expires = expiration.ToTtl(),
                Completed = s => 
                {  
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(touchOp, tcs, token, timeout).ConfigureAwait(false);
        }

        public Task Touch(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        }

        public async Task<IMutationResult> MutateIn(string id, OperationSpec[] specs, Action<MutateInOptions> options = default(Action<MutateInOptions>))
        {
            var mutateInOptions = new MutateInOptions();
            options?.Invoke(mutateInOptions);

            // convert new style specs into old style builder
            var builder = new MutateInBuilder<byte[]>(null, null, id);
            foreach (var spec in specs)
            {
                switch (spec.OpCode)
                {
                    case OpCode.SubDictAdd:
                        builder.Insert(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubDictUpsert:
                        builder.Upsert(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubReplace:
                        builder.Replace(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubDelete:
                        builder.Remove(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubArrayPushLast:
                        builder.ArrayAppend(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayPushFirst:
                        builder.ArrayPrepend(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayInsert:
                        builder.ArrayInsert(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayAddUnique:
                        builder.ArrayAddUnique(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubCounter:
                        builder.Counter(spec.Path, (long) spec.Value, spec.PathFlags.HasFlag(SubdocPathFlags.CreatePath));
                        break;

                    default:
                        // unknown opcode?
                        break;
                }
            }

            var tcs = new TaskCompletionSource<byte[]>();
            var mutation = new MultiMutation<byte[]>
            {
                Key = id,
                Builder = builder,
                VBucketId = 752, // hack, this needs to be set properly
                Cid = Cid,
                Completed = s =>
                {
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(mutation, tcs);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await mutation.ReadAsync(bytes).ConfigureAwait(false);
            return new MutationResult(mutation.Cas, null, mutation.MutationToken);
        }

        public async Task<IMutationResult> MutateIn(string id, Action<MutateInSpec> ops, Action<MutateInOptions> options = null)
        {
            var mutateInSpec = new MutateInSpec();
            ops(mutateInSpec);

            var mutateInOptions = new MutateInOptions();
            options?.Invoke(mutateInOptions);

            // convert new style specs into old style builder
            var builder = new MutateInBuilder<byte[]>(null, null, id);
            foreach (var spec in mutateInSpec.Specs)
            {
                switch (spec.OpCode)
                {
                    case OpCode.SubDictAdd:
                        builder.Insert(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubDictUpsert:
                        builder.Upsert(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubReplace:
                        builder.Replace(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubDelete:
                        builder.Remove(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubArrayPushLast:
                        builder.ArrayAppend(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayPushFirst:
                        builder.ArrayPrepend(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayInsert:
                        builder.ArrayInsert(spec.Path, spec.PathFlags, spec.DocFlags, spec.Value);
                        break;
                    case OpCode.SubArrayAddUnique:
                        builder.ArrayAddUnique(spec.Path, spec.Value, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubCounter:
                        builder.Counter(spec.Path, (long) spec.Value, spec.PathFlags.HasFlag(SubdocPathFlags.CreatePath));
                        break;

                    default:
                        // unknown opcode?
                        break;
                }
            }

            var tcs = new TaskCompletionSource<byte[]>();
            var mutation = new MultiMutation<byte[]>
            {
                Key = id,
                Builder = builder,
                VBucketId = 752, // hack, this needs to be set properly
                Cid = Cid,
                Completed = s =>
                {
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(mutation, tcs);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await mutation.ReadAsync(bytes).ConfigureAwait(false);
            return new MutationResult(mutation.Cas, null, mutation.MutationToken);
        }

        public async Task<ILookupInResult> LookupIn(string id, Action<LookupInSpec> ops, Action<LookupInOptions> options = default(Action<LookupInOptions>))
        {
            var lookupInSpec = new LookupInSpec();
            ops(lookupInSpec);

            var lookupInOptions = new LookupInOptions();
            options?.Invoke(lookupInOptions);

            // convert new style specs into old style builder
            var builder = new LookupInBuilder<byte[]>(null, null, id);
            foreach (var spec in lookupInSpec.Specs)
            {
                switch (spec.OpCode)
                {
                    case OpCode.SubGet:
                        builder.Get(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubExist:
                        builder.Exists(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    default:
                        // unknown opcode?
                        break;
                }
            }

            var tcs = new TaskCompletionSource<byte[]>();
            var lookup = new MultiLookup<byte[]>
            {
                Key = id,
                Builder = builder,
                VBucketId = 752, // hack, this needs to be set properly
                Cid = Cid,
                Completed = s =>
                {
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(lookup, tcs);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await lookup.ReadAsync(bytes).ConfigureAwait(false);
            return new LookupInResult(bytes, lookup.Cas, null);
        }

        public async Task<ILookupInResult> LookupIn(string id, OperationSpec[] specs, Action<LookupInOptions> options = default(Action<LookupInOptions>))
        {
            var lookupInOptions = new LookupInOptions();
            options?.Invoke(lookupInOptions);

            // convert new style specs into old style builder
            var builder = new LookupInBuilder<byte[]>(null, null, id);
            foreach (var spec in specs)
            {
                switch (spec.OpCode)
                {
                    case OpCode.SubGet:
                        builder.Get(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    case OpCode.SubExist:
                        builder.Exists(spec.Path, spec.PathFlags, spec.DocFlags);
                        break;
                    default:
                        // unknown opcode?
                        break;
                }
            }

            var tcs = new TaskCompletionSource<byte[]>();
            var lookup = new MultiLookup<byte[]>
            {
                Key = id,
                Builder = builder,
                VBucketId = 752, // hack, this needs to be set properly
                Cid = Cid,
                Completed = s =>
                {
                    if (s.Status == ResponseStatus.Success)
                    {
                        tcs.SetResult(s.Data.ToArray());
                    }
                    else
                    {
                        tcs.SetException(new Exception(s.Status.ToString()));
                    }

                    return tcs.Task;
                }
            };

            await ExecuteOp(lookup, tcs);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await lookup.ReadAsync(bytes).ConfigureAwait(false);
            return new LookupInResult(bytes, lookup.Cas, null);
        }
    }
}
