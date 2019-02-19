using System.Collections.Generic;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public class MutateInSpec
    {
        internal readonly List<OperationSpec> Specs = new List<OperationSpec>();

        public MutateInSpec Insert<T>(string path, T value, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.Insert(path, value, flags));
            return this;
        }

        public MutateInSpec Insert<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.Insert(path, value, createPath, xAttr));
            return this;
        }

        public MutateInSpec Upsert<T>(string path, T value, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.Upsert(path, value, flags));
            return this;
        }

        public MutateInSpec Upsert<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.Upsert(path, value, createPath, xAttr));
            return this;
        }

        public MutateInSpec Replace<T>(string path, T value, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.Replace<T>(path, value, flags));
            return this;
        }

        public MutateInSpec Replace<T>(string path, T value, bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.Replace(path, value, xAttr));
            return this;
        }

        public MutateInSpec Remove(string path, bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.Remove(path, xAttr));
            return this;
        }

        public MutateInSpec ArrayAppend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.ArrayAppend(path, values, flags));
            return this;
        }

        public MutateInSpec ArrayAppend<T>(string path, T[] values, bool createPath = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.ArrayAppend(path, values, createPath, xAttr));
            return this;
        }

        public MutateInSpec ArrayPrepend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.ArrayPrepend(path, values, flags));
            return this;
        }

        public MutateInSpec ArrayPrepend<T>(string path, T[] values, bool createParents = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.ArrayPrepend(path, values, createParents, xAttr));
            return this;
        }

        public MutateInSpec ArrayInsert<T>(string path, T[] values, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.ArrayInsert(path, values, flags));
            return this;
        }

        public MutateInSpec ArrayInsert<T>(string path, T[] values, bool createParents= default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.ArrayInsert(path, values, createParents, xAttr));
            return this;
        }

        public MutateInSpec ArrayAddUnique<T>(string path, T value, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.ArrayAddUnique(path, value, flags));
            return this;
        }

        public MutateInSpec ArrayAddUnique<T>(string path, T value, bool createPath = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.ArrayAddUnique(path, value, createPath, xAttr));
            return this;
        }

        public MutateInSpec Counter(string path, long delta, SubdocPathFlags flags)
        {
            Specs.Add(MutateInSpecs.Counter(path, delta, flags));
            return this;
        }

        public MutateInSpec Counter(string path, long delta, bool createPath = default(bool), bool xAttr = default(bool))
        {
            Specs.Add(MutateInSpecs.Counter(path, delta, createPath, xAttr));
            return this;
        }
    }

    public static class MutateInSpecs
    {
        private static OperationSpec AddSpec(OpCode opCode, string path, object value, SubdocPathFlags flags)
        {
            return new OperationSpec
            {
                Path = path,
                Value = value,
                OpCode = opCode,
                PathFlags = flags
            };
        }

        private static OperationSpec AddSpec(OpCode opCode, string path, object value, bool createPath = default(bool), bool xAttr = default(bool))
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
            return AddSpec(opCode, path, value, pathFlags);
        }

        public static OperationSpec Insert<T>(string path, T value, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubGet, path, value, flags);
        }

        public static OperationSpec Insert<T>(string path, T value, bool createPath, bool xAttr)
        {
            return AddSpec(OpCode.SubGet, path, value, createPath, xAttr);
        }

        public static OperationSpec Upsert<T>(string path, T value, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubDictUpsert, path, value, flags);
        }

        public static OperationSpec Upsert<T>(string path, T value, bool createPath, bool xAttr)
        {
            return AddSpec(OpCode.SubDictUpsert, path, value, createPath, xAttr);
        }

        public static OperationSpec Replace<T>(string path, T value, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubReplace, path, value, flags);
        }

        public static OperationSpec Replace<T>(string path, T value, bool xAttr)
        {
            return AddSpec(OpCode.SubReplace, path, value, false, xAttr);
        }

        public static OperationSpec Remove(string path, bool xAttr)
        {
            return AddSpec(OpCode.SubDelete, path, null, false, xAttr);
        }

        public static OperationSpec ArrayAppend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubArrayPushLast, path, values, flags);
        }

        public static OperationSpec ArrayAppend<T>(string path, T[] values, bool createPath, bool xAttr)
        {
            return AddSpec(OpCode.SubArrayPushLast, path, values, createPath, xAttr);
        }

        public static OperationSpec ArrayPrepend<T>(string path, T[] values, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubArrayPushFirst, path, values, flags);
        }

        public static OperationSpec ArrayPrepend<T>(string path, T[] values, bool createParents, bool xAttr)
        {
            return AddSpec(OpCode.SubArrayPushFirst, path, values, createParents, xAttr);
        }

        public static OperationSpec ArrayInsert<T>(string path, T[] values, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubArrayInsert, path, values, flags);
        }

        public static OperationSpec ArrayInsert<T>(string path, T[] values, bool createParents, bool xAttr)
        {
            return AddSpec(OpCode.SubArrayInsert, path, values, createParents, xAttr);
        }

        public static OperationSpec ArrayAddUnique<T>(string path, T value, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubArrayAddUnique, path, value, flags);
        }

        public static OperationSpec ArrayAddUnique<T>(string path, T value, bool createPath, bool xAttr)
        {
            return AddSpec(OpCode.SubArrayAddUnique, path, value, createPath, xAttr);
        }

        public static OperationSpec Counter(string path, long delta, SubdocPathFlags flags)
        {
            return AddSpec(OpCode.SubArrayAddUnique, path, delta, flags);
        }

        public static OperationSpec Counter(string path, long delta, bool createPath, bool xAttr)
        {
            return AddSpec(OpCode.SubArrayAddUnique, path, delta, createPath, xAttr);
        }
    }
}
