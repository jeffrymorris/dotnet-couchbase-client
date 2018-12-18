using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase
{
    public class MutationSpec
    {
        public MutationSpec Insert<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec Upsert<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec Replace<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec Remove<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec ArrayAppend<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec ArrayPrepend<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec ArrayInsert<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec ArrayAddUnique<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public MutationSpec Counter<TValue>(string path, TValue value, Action<MutateOptions> options = null)
        {
            throw new NotImplementedException();
        }
    }
}
