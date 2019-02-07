using System.Collections.Generic;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Operations.SubDocument;

namespace Couchbase
{
    public class LookupInSpec
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

        private void AddSpec(OpCode opCode, string path, object value, bool xAttr = default(bool))
        {
            var pathFlags = SubdocPathFlags.None;
            if (xAttr)
            {
                pathFlags = SubdocPathFlags.Xattr;
            }
            AddSpec(opCode, path, value, pathFlags);
        }

        public LookupInSpec Path(string path)
        {
            AddSpec(OpCode.SubGet, path, false);
            return this;
        }

        public LookupInSpec Exists(string path)
        {
            AddSpec(OpCode.SubExist, path, false);
            return this;
        }

        public LookupInSpec XAttr(string path)
        {
            AddSpec(OpCode.SubGet, path, true);
            return this;
        }
    }
}
