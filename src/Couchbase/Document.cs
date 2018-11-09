using System;
using System.Threading.Tasks;
using Couchbase.Core;

namespace Couchbase
{
    public class Document<TBody> : IDocument<TBody>
    {
        public string Id { get; set; }
        public TimeSpan Expiry { get; set; }
        public long Cas { get; }
        public MutationToken MutationToken { get; }
        public TBody Body { get; set; }

        public TBody GetBody()
        {
            throw new NotImplementedException();
        }

        public Task SetBody(TBody json)
        {
            throw new NotImplementedException();
        }

        public TValue Get<TValue>(string path, Action<SubDocFetchOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string path, Action<SubDocFetchOptions> options)
        {
            throw new NotImplementedException();
        }

        public bool Count(string path, Action<SubDocFetchOptions> options)
        {
            throw new NotImplementedException();
        }

        public void Insert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void Upsert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void Replace<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void Remove<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void ArrayAppend<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void ArrayPrepend<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void ArrayInsert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void ArrayAddUnique<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void Counter<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public void SetAttribute<TValue>(string path, TValue value, Action<XAttrOptions> options = null)
        {
            throw new NotImplementedException();
        }

        public TValue GetAttribute<TValue>(string path)
        {
            throw new NotImplementedException();
        }

        public TValue GetVirtualAttribute<TValue>(string path)
        {
            throw new NotImplementedException();
        }
    }
}