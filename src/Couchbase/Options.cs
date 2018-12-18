using System;
using System.Collections.Generic;
using System.Dynamic;

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

        public DurabilityLevel  DurabilityLevel{ get; set; }
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
        public bool IncludeExpiration { get; set; }

        public bool CreatePath { get; set; }

        public TimeSpan Timeout { get; set; }

        public List<string> ProjectList { get; set; }

        public GetOptions WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public GetOptions Project(params string[] fields)
        {
            if(ProjectList == null) ProjectList = new List<string>();
            ProjectList.AddRange(fields);
            return this;
        }

        public GetOptions WithCreatePath(bool createPath)
        {
            CreatePath = createPath;
            return this;
        }

        public GetOptions WithExpiration()
        {
            IncludeExpiration = true;
            return this;
        }
    }

    #endregion

    public class XAttrOptions
    {
        public bool CreatePath { get; set; }

        public bool CreateDocument { get; set; }

        //add more
    }

    public struct LookupInOptions
    {
        public LookupInOptions Path(string path)
        {
            return this;
        }

        public LookupInOptions Exists(string path)
        {
            return this;
        }

        public LookupInOptions XAttr(string path)
        {
            return this;
        }

        public LookupInOptions Timeout(TimeSpan timeout)
        {
            return this;
        } 
    }

    public struct MutateOptions
    {
        internal bool _CreatePath { get; set; }
        internal TimeSpan _Timeout { get; set; }
        internal Dictionary<string, object> _ops;

        public MutateOptions CreatePath(bool createPath)
        {
            _CreatePath = createPath;
            return this;
        }

        public MutateOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        public MutateOptions Upsert<T>(string path, T value)
        {
            if(_ops == null) _ops = new Dictionary<string, object>();
            _ops.Add(path, value);
            return this;
        }

        public MutateOptions Insert<T>(string path, T value)
        {
            if(_ops == null) _ops = new Dictionary<string, object>();
            _ops.Add(path, value);
            return this;
        }

        public MutateOptions Replace<T>(string path, T value)
        {
            return this;
        }

        public MutateOptions Remove(string path)
        {
            return this;
        }

        public MutateOptions ArrayAppend(string path, params object[] values)
        {
            return this;
        }

        public MutateOptions ArrayPrepend(string path, params object[] values)
        {
            return this;
        }

        public MutateOptions ArrayInsert(string path, params object[] values)
        {
            return this;
        }

        public MutateOptions ArrayAddUnique(string path, params object[] values)
        {
            return this;
        }
    }
}
