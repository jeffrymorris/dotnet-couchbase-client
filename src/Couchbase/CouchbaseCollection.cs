using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.Legacy;
using Couchbase.Utils;
using System;
using System.Collections.Generic;
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

        #region LookupIn

        private static void ConfigureLookupInOptions(LookupInOptions options, TimeSpan? timeout)
        {
            if (timeout.HasValue)
            {
                options.Timeout(timeout.Value);
            }
        }

        public Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, TimeSpan? timeout = null)
        {
            var builder = new LookupInSpecBuilder();
            configureBuilder(builder);

            var options = new LookupInOptions();
            ConfigureLookupInOptions(options, timeout);

            return LookupIn(id, builder.Specs, options);
        }

        public Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, Action<LookupInOptions> configureOptions)
        {
            var builder = new LookupInSpecBuilder();
            configureBuilder(builder);

            var options = new LookupInOptions();
            configureOptions(options);

            return LookupIn(id, builder.Specs, options);
        }

        public Task<ILookupInResult> LookupIn(string id, Action<LookupInSpecBuilder> configureBuilder, LookupInOptions options)
        {
            var lookupInSpec = new LookupInSpecBuilder();
            configureBuilder(lookupInSpec);

            return LookupIn(id, lookupInSpec.Specs, options);
        }

        public Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null)
        {
            var options = new LookupInOptions();
            ConfigureLookupInOptions(options, timeout);

            return LookupIn(id, specs, options);
        }

        public Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, Action<LookupInOptions> configureOptions)
        {
            var options = new LookupInOptions();
            configureOptions(options);

            return LookupIn(id, specs, options);
        }

        public async Task<ILookupInResult> LookupIn(string id, IEnumerable<OperationSpec> specs, LookupInOptions options)
        {
            // use default timeout if not set
            if (options._Timeout == TimeSpan.Zero)
            {
                options.Timeout(DefaultTimeout);
            }

            // convert new style specs into old style builder
            var builder = new LookupInBuilder<byte[]>(null, null, id, specs);

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

            await ExecuteOp(lookup, tcs, timeout: options._Timeout);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await lookup.ReadAsync(bytes).ConfigureAwait(false);
            return new LookupInResult(bytes, lookup.Cas, null);
        }

        #endregion

        #region MutateIn

        private static void ConfigureMutateInOptions(MutateInOptions options, TimeSpan? timeout, TimeSpan? expiration, ulong cas, bool createDocument)
        {
            if (timeout.HasValue)
            {
                options.Timeout(timeout.Value);
            }

            if (expiration.HasValue)
            {
                options.Expiration(expiration.Value);
            }

            if (cas > 0)
            {
                options.Cas(cas);
            }

            var flags = SubdocDocFlags.None;
            if (createDocument)
            {
                flags ^= SubdocDocFlags.UpsertDocument;
            }

            options.Flags(flags);
        }

        public Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false)
        {
            var builder = new MutateInSpecBuilder();
            configureBuilder(builder);

            var options = new MutateInOptions();

            ConfigureMutateInOptions(options, timeout, expiration, cas, createDocument);

            return MutateIn(id, builder.Specs, options);
        }

        public Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, Action<MutateInOptions> configureOptions)
        {
            var builder = new MutateInSpecBuilder();
            configureBuilder(builder);

            var options = new MutateInOptions();
            configureOptions(options);
            
            return MutateIn(id, builder.Specs, options);
        }

        public Task<IMutationResult> MutateIn(string id, Action<MutateInSpecBuilder> configureBuilder, MutateInOptions options)
        {
            var mutateInSpec = new MutateInSpecBuilder();
            configureBuilder(mutateInSpec);

            return MutateIn(id, mutateInSpec.Specs, options);
        }

        public Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, TimeSpan? timeout = null, TimeSpan? expiration = null, ulong cas = 0, bool createDocument = false)
        {
            var options = new MutateInOptions();
            ConfigureMutateInOptions(options, timeout, expiration, cas, createDocument);

            return MutateIn(id, specs, options);
        }

        public Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, Action<MutateInOptions> configureOptions)
        {
            var options = new MutateInOptions();
            configureOptions(options);

            return MutateIn(id, specs, options);
        }

        public async Task<IMutationResult> MutateIn(string id, IEnumerable<OperationSpec> specs, MutateInOptions options)
        {
            // use default timeout if not set
            if (options._Timeout == TimeSpan.Zero)
            {
                options.Timeout(DefaultTimeout);
            }

            // convert new style specs into old style builder
            var builder = new MutateInBuilder<byte[]>(null, null, id, specs);

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

            await ExecuteOp(mutation, tcs, timeout: options._Timeout);
            var bytes = await tcs.Task.ConfigureAwait(false);
            await mutation.ReadAsync(bytes).ConfigureAwait(false);
            return new MutationResult(mutation.Cas, null, mutation.MutationToken);
        }

        #endregion
    }
}
