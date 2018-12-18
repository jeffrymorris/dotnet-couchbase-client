﻿using System;
using System.Threading.Tasks;

namespace Couchbase
{
    public class CouchbaseBucket : IBucket
    {
        public CouchbaseBucket(ICluster cluster, string name)
        {
            //_cluster = cluster;
            Name = name;
        }

        public string Name { get; }

        public Task BootstrapAsync(Uri uri)
        {
            throw new NotImplementedException();
        }

        public ICollection this[string name] => throw new NotImplementedException();

        public ICollection GetCollection(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection GetCollection(string scope, string name)
        {
            throw new NotImplementedException();
        }

        #region GET
        public Task<IDocument<T>> Get<T>(string id, TimeSpan timeSpan = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Get<T>(string id, GetOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Get<T>(string id, Action<GetOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndLock<T>(string id, TimeSpan expiration, TimeSpan timeSpan = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndLock<T>(string id, GetAndLockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndLock<T>(string id, Action<GetAndLockOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndTouch<T>(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndTouch<T>(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> GetAndTouch<T>(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Upsert
        public Task<IDocument<T>> Upsert<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
           uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Upsert<T>(IDocument<T> document, UpsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Upsert<T>(IDocument<T> document, Action<UpsertOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Insert
        public Task<IDocument<T>> Insert<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
           uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Insert<T>(IDocument<T> document, InsertOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Insert<T>(IDocument<T> document, Action<InsertOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Replace
        public Task<IDocument<T>> Replace<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0, PersistTo persistTo = PersistTo.Zero, ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Replace<T>(IDocument<T> document, ReplaceOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<T>> Replace<T>(IDocument<T> document, Action<ReplaceOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public Task<IDocument<T>> Remove<T>(string id, TimeSpan timeSpan = new TimeSpan(), uint cas = 0, PersistTo persistTo = PersistTo.Zero,
         ReplicateTo replicateTo = ReplicateTo.Zero)
        {
            throw new NotImplementedException();
        }

        public Task Remove<T>(string id, RemoveOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Remove<T>(string id, Action<RemoveOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Increment
        public Task Increment(string id, ulong delta, ulong initial, TimeSpan timeout = new TimeSpan(),
           TimeSpan expiration = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<ulong>> Increment(string id, IncrementOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<ulong>> Increment(string id, Action<IncrementOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Decrement
        public Task<IDocument<ulong>> Decrement(string id, ulong delta, ulong initial, TimeSpan timeSpan = new TimeSpan(),
           TimeSpan expiration = new TimeSpan(), uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<ulong>> Decrement(string id, DecrementOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<ulong>> Decrement(string id, Action<DecrementOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Append
        public Task<IDocument<string>> Append(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
           uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<string>> Append(string id, string value, AppendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<string>> Append(string id, string value, Action<AppendOptions> options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<string>> Append(string id, byte[] value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<byte[]>> Append(string id, byte[] value, AppendOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<byte[]>> Append(string id, byte[] value, Action<AppendOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Prepend

        public Task<IDocument<byte[]>> Prepend(string id, string value, TimeSpan timeSpan = new TimeSpan(), TimeSpan expiration = new TimeSpan(),
            uint cas = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<string>> Prepend(string id, string value, PrependOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IDocument<string>> Prepend(string id, string value, Action<PrependOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Unlock
        public Task Unlock<T>(IDocument<T> document, TimeSpan timeSpan = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(IDocument<T> document, UnlockOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Unlock<T>(IDocument<T> document, Action<UnlockOptions> options)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Touch
        public Task Touch(string id, TimeSpan expiration, TimeSpan timeout = new TimeSpan())
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, GetAndTouchOptions options)
        {
            throw new NotImplementedException();
        }

        public Task Touch(string id, Action<GetAndTouchOptions> options)
        {
            throw new NotImplementedException();
        } 
        #endregion

        public void LoadManifest(string path)
        {
            throw new NotImplementedException();
        }
    }
}