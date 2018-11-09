using System;
using Couchbase.Core;

namespace Couchbase
{
    public interface IDocument
    {
        #region Document Properties

        string Id { get; set; }

        TimeSpan Expiry { get; set; }
        
        long Cas { get; }

        MutationToken Token { get; }

        #endregion

        #region Sub-Document fetch

        TValue Get<TValue>(string path, Action<SubDocFetchOptions> options = null);

        bool Exists(string path, Action<SubDocFetchOptions> options);

        bool Count(string path, Action<SubDocFetchOptions> options);

        #endregion

        #region Sub-Document mutate

        void Insert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void Upsert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void Replace<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void Remove<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void ArrayAppend<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void ArrayPrepend<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void ArrayInsert<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void ArrayAddUnique<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        void Counter<TValue>(string path, TValue value, Action<SubDocMutateOptions> options = null);

        #endregion;

        #region XAttrs & virtual XAttrs

        void SetAttribute<TValue>(string path, TValue value, Action<XAttrOptions> options = null);
            
        TValue GetAttribute<TValue>(string path);

        TValue GetVirtualAttribute<TValue>(string path);

        #endregion
    }
}
