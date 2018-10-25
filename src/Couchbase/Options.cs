using System;

namespace Couchbase
{
    #region Insert Options
    public class InsertOptions
    {
        internal TimeSpan _Timeout;
        public InsertOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public InsertOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }

        internal long _Cas;
        public InsertOptions Cas(long cas)
        {
            _Cas = cas;
            return this;
        }

        internal ReplicateTo _ReplicateTo;
        public InsertOptions ReplicateTo(ReplicateTo replicateTo)
        {
            _ReplicateTo = replicateTo;
            return this;
        }

        internal PersistTo _PersistTo;
        public InsertOptions PersistTo(PersistTo persistTo)
        {
            _PersistTo = persistTo;
            return this;
        }
    }
    #endregion

    #region Upsert Options
    public class UpsertOptions
    {
        internal TimeSpan _Timeout;
        public UpsertOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public UpsertOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }

        internal long _Cas;
        public UpsertOptions Cas(long cas)
        {
            _Cas = cas;
            return this;
        }

        internal ReplicateTo _ReplicateTo;
        public UpsertOptions ReplicateTo(ReplicateTo replicateTo)
        {
            _ReplicateTo = replicateTo;
            return this;
        }

        internal PersistTo _PersistTo;
        public UpsertOptions PersistTo(PersistTo persistTo)
        {
            _PersistTo = persistTo;
            return this;
        }
    }
    #endregion

    #region Remove Options
    public class RemoveOptions
    {
        internal TimeSpan _Timeout;
        public RemoveOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal long _Cas;
        public RemoveOptions Cas(long cas)
        {
            _Cas = cas;
            return this;
        }

        internal ReplicateTo _ReplicateTo;
        public RemoveOptions ReplicateTo(ReplicateTo replicateTo)
        {
            _ReplicateTo = replicateTo;
            return this;
        }

        internal PersistTo _PersistTo;
        public RemoveOptions PersistTo(PersistTo persistTo)
        {
            _PersistTo = persistTo;
            return this;
        }
    }

    #endregion

    #region Replace Options
    public class ReplaceOptions
    {
        internal TimeSpan _Timeout;
        public ReplaceOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public ReplaceOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }

        internal long _Cas;
        public ReplaceOptions Cas(long cas)
        {
            _Cas = cas;
            return this;
        }

        internal ReplicateTo _ReplicateTo;
        public ReplaceOptions ReplicateTo(ReplicateTo replicateTo)
        {
            _ReplicateTo = replicateTo;
            return this;
        }

        internal PersistTo _PersistTo;
        public ReplaceOptions PersistTo(PersistTo persistTo)
        {
            _PersistTo = persistTo;
            return this;
        }
    }
    #endregion

    #region Increment Options

    public class IncrementOptions
    {
        internal TimeSpan _Timeout;
        public IncrementOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public IncrementOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }

        internal ulong _Initial;
        public IncrementOptions Initial(ulong initial)
        {
            _Initial = initial;
            return this;
        }

        internal ulong _Delta;
        public IncrementOptions Delta(ulong delta)
        {
            _Delta = delta;
            return this;
        }
    }

    #endregion

    #region Decrement options

    public class DecrementOptions
    {
        internal TimeSpan _Timeout;
        public DecrementOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public DecrementOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }

        internal ulong _Initial;
        public DecrementOptions Initial(ulong initial)
        {
            _Initial = initial;
            return this;
        }

        internal ulong _Delta;
        public DecrementOptions Delta(ulong delta)
        {
            _Delta = delta;
            return this;
        }
    }
    #endregion

    #region Append Options

    public class AppendOptions
    {
        internal TimeSpan _Timeout;
        public AppendOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public AppendOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }
    }

    #endregion

    #region Prepend Options

    public class PrependOptions
    {
        internal TimeSpan _Timeout;
        public PrependOptions Timeout(TimeSpan timeout)
        {
            _Timeout = timeout;
            return this;
        }

        internal TimeSpan _Expiration;
        public PrependOptions Expiration(TimeSpan expiration)
        {
            _Expiration = expiration;
            return this;
        }
    }
    
    #endregion

    #region GetAndLock Options

    public class GetAndLockOptions
    {
        protected internal TimeSpan _Timeout;
        public GetAndLockOptions Timeout(TimeSpan timeout)
        {
            return this;
        }
    }

    #endregion

    #region Unlock Options

    public class UnlockOptions
    {
        protected internal TimeSpan _Timeout;
        public UnlockOptions Timeout(TimeSpan timeout)
        {
            return this;
        }
    }

    #endregion

    #region GetAndTouch Options

    public class GetAndTouchOptions
    {
        protected internal TimeSpan _Timeout;
        public GetAndTouchOptions Timeout(TimeSpan timeout)
        {
            return this;
        }
    }

    #endregion

    #region Touch Options

    public class TouchOptions
    {
        protected internal TimeSpan _Timeout;
        public TouchOptions Timeout(TimeSpan timeout)
        {
            return this;
        }
    }

    #endregion

    #region GetOptions

    public class GetOptions
    {
        protected internal TimeSpan _Timeout;
        public GetOptions Timeout(TimeSpan timeout)
        {
            return this;
        }
    }

    #endregion
}
