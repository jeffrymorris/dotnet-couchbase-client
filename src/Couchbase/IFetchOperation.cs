using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core.IO;
using Couchbase.Core.IO.Operations;
using Couchbase.Core.IO.Serializers;

namespace Couchbase
{
    public interface IFetchOperation<T> : IOperation<T>
    {
        IFetchOperation<T> WithTimeout(TimeSpan timeout);

        IFetchOperation<T> WithCas(uint cas);

        IFetchOperation<T> WithDurability(PersistTo peristTo, ReplicateTo replicateTo);

        //override default transcoder
        IFetchOperation<T> WithTranscoder(ITranscoder transcoder);

        //override the default serializer
        IFetchOperation<T> WithSerializer(ITypeSerializer serializer);

            //should return type be the document?
        Task<IDocument<T>> ExecuteAsync();
    }
}
