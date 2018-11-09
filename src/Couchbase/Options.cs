using System;

namespace Couchbase
{
    #region Insert Options

    public class InsertOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public long Cas { get; set; }

        public ReplicateTo ReplicateTo { get; set; }

        public PersistTo PersistTo { get; set; }
    }
    #endregion

    #region Upsert Options
    public class UpsertOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public long Cas { get; set; }

        public ReplicateTo ReplicateTo { get; set; }

        public PersistTo PersistTo { get; set; }
    }
    #endregion

    #region Remove Options
    public class RemoveOptions
    {
        public TimeSpan Timeout { get; set; }

        public long Cas { get; set; }

        public ReplicateTo ReplicateTo { get; set; }

        public PersistTo PersistTo { get; set; }
    }

    #endregion

    #region Replace Options
    public class ReplaceOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public long Cas { get; set; }

        public ReplicateTo ReplicateTo { get; set; }

        public PersistTo PersistTo { get; set; }
    }
    #endregion

    #region Increment Options

    public class IncrementOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public ulong Initial { get; set; }

        public ulong Delta { get; set; }
    }

    #endregion

    #region Decrement options

    public class DecrementOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }

        public ulong Initial { get; set; }

        public ulong Delta { get; set; }
    }
    #endregion

    #region Append Options

    public class AppendOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }
    }

    #endregion

    #region Prepend Options

    public class PrependOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }
    }
    
    #endregion

    #region GetAndLock Options

    public class GetAndLockOptions
    {
        public TimeSpan Timeout { get; set; }

        public TimeSpan Expiration { get; set; }
    }

    #endregion

    #region Unlock Options

    public class UnlockOptions
    {
        public TimeSpan Timeout { get; set; }
    }

    #endregion

    #region GetAndTouch Options

    public class GetAndTouchOptions
    {
        public TimeSpan Timeout { get; set; }
    }

    #endregion

    #region Touch Options

    public class TouchOptions
    {
        public TimeSpan Timeout { get; set; }
    }

    #endregion

    #region GetOptions

    public class GetOptions
    {
        public TimeSpan Timeout { get; set; }

        public bool IncludeBody { get; set; }
    }

    #endregion

    public class XAttrOptions
    {
        public bool CreatePath { get; set; }

        public bool CreateDocument { get; set; }

        //add more
    }

    public class SubDocFetchOptions
    {
        public bool CreatePath { get; set; }

        //add more
    }

    public class SubDocMutateOptions
    {
        public bool CreatePath { get; set; }

        //add more
    }
}
