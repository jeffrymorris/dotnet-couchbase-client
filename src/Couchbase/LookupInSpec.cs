using System.Collections.Generic;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public class LookupInSpec
    {
        internal readonly List<OperationSpec> Specs = new List<OperationSpec>();

        public LookupInSpec Path(string path)
        {
            Specs.Add(LookupInSpecs.Path(path));
            return this;
        }

        public LookupInSpec Exists(string path)
        {
            Specs.Add(LookupInSpecs.Exists(path));
            return this;
        }

        public LookupInSpec XAttr(string path)
        {
            Specs.Add(LookupInSpecs.XAttr(path));
            return this;
        }
    }

    public static class LookupInSpecs
    {
        private static OperationSpec AddSpec(OpCode opCode, string path, SubdocPathFlags flags = SubdocPathFlags.None)
        {
            return new OperationSpec
            {
                Path = path,
                OpCode = opCode,
                PathFlags = flags
            };
        }

        public static OperationSpec Path(string path)
        {
            return AddSpec(OpCode.SubGet, path);
        }

        public static OperationSpec Exists(string path)
        {
            return AddSpec(OpCode.SubExist, path);
        }

        public static OperationSpec XAttr(string path)
        {
            return AddSpec(OpCode.SubGet, path, SubdocPathFlags.Xattr);
        }
    }
}
