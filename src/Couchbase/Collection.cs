using System.ComponentModel;
using Couchbase.Core.IO.Converters;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Transcoders;

namespace Couchbase
{
    public class Collection : Couchbase.ICollection
    {
        public Collection(IBucket bucket, string cid, string name)
        {
            Bucket = bucket;
            Cid = cid;
            Name = name;
        }

        protected IBucket Bucket { get; }

        public string Cid { get; }

        public string Name { get; }

        public IMutationOperation Upsert<T>(IDocument<T> document)
        {
            return new Upsert();
        }

        public IMutationOperation Insert<T>(IDocument<T> document)
        {
            return new Set<T>(Bucket, document)
            {
                Cid = Cid,
                Converter = new DefaultConverter(),
                Transcoder = new DefaultTranscoder(new DefaultConverter()),
                Opaque = SequenceGenerator.GetNext(),
                Body = document.Content
            };
        }

        public IMutationOperation Replace<T>(IDocument<T> document)
        {
            throw new System.NotImplementedException();
        }

        public IMutationOperation Remove<T>(IDocument<T> document)
        {
            throw new System.NotImplementedException();
        }

        public IFetchOperation<T> Get<T>(string key)
        {
            return new Get<T>(Bucket)
            {
                Cid =  Cid
            };
        }

        public IQueryResponse Query(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}
