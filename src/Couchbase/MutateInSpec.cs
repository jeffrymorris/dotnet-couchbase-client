using System.Collections.Generic;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public class MutateInSpec
    {
        internal readonly Dictionary<string, OperationSpec> Specs = new Dictionary<string, OperationSpec>();

        private void AddSpec(OpCode opCode, string path, object value, SubdocPathFlags flags)
        {
            Specs.Add(path, new OperationSpec
            {
                Path = path,
                Value = value,
                OpCode = opCode,
                PathFlags = flags
            });
        }

        private void AddSpec(OpCode opCode, string path, object value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            var pathFlags = SubdocPathFlags.None;
            if (createPath || xAttr)
            {
                if (createPath)
                {
                    pathFlags = SubdocPathFlags.CreatePath;
                }
                if (xAttr)
                {
                    pathFlags = pathFlags | SubdocPathFlags.Xattr;
                }
            }
            AddSpec(opCode, path, value, pathFlags);
        }

        public MutateInSpec Insert<T>(string path, T value, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubGet, path, value, flags);
            return this;
        }

        public MutateInSpec Insert<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubGet, path, value, createPath, xAttr);
            return this;
        }

        public MutateInSpec Upsert<T>(string path, T value, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubDictUpsert, path, value, flags);
            return this;
        }

        public MutateInSpec Upsert<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubDictUpsert, path, value, createPath, xAttr);
            return this;
        }

        public MutateInSpec Replace<T>(string path, T value, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubReplace, path, value, flags);
            return this;
        }

        public MutateInSpec Replace<T>(string path, T value, bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubReplace, path, value, false, xAttr);
            return this;
        }

        public MutateInSpec Remove(string path, bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubDelete, path, null, false, xAttr);
            return this;
        }

        public MutateInSpec ArrayAppend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubArrayPushLast, path, values, flags);
            return this;
        }

        public MutateInSpec ArrayAppend<T>(string path, T[] values, bool createPath = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubArrayPushLast, path, values, createPath, xAttr);
            return this;
        }

        public MutateInSpec ArrayPrepend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubArrayPushFirst, path, values, flags);
            return this;
        }

        public MutateInSpec ArrayPrepend<T>(string path, T[] values, bool createParents = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubArrayPushFirst, path, values, createParents, xAttr);
            return this;
        }

        public MutateInSpec ArrayInsert<T>(string path, T[] values, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubArrayInsert, path, values, flags);
            return this;
        }

        public MutateInSpec ArrayInsert<T>(string path, T[] values, bool createParents= default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubArrayInsert, path, values, createParents, xAttr);
            return this;
        }

        public MutateInSpec ArrayAddUnique<T>(string path, T value, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubArrayAddUnique, path, value, flags);
            return this;
        }

        public MutateInSpec ArrayAddUnique<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubArrayAddUnique, path, value, createPath, xAttr);
            return this;
        }

        public MutateInSpec Counter(string path, long delta, SubdocPathFlags flags)
        {
            AddSpec(OpCode.SubArrayAddUnique, path, delta, flags);
            return this;
        }

        public MutateInSpec Counter(string path, long delta, bool createPath = default(bool), bool xAttr = default(bool))
        {
            AddSpec(OpCode.SubArrayAddUnique, path, delta, createPath, xAttr);
            return this;
        }
    }
}
