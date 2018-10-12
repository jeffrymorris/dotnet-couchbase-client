namespace Couchbase
{
    public class Document<T> : IDocument<T>
    {
        public string Key { get; set; }

        public T Content { get; set; }
    }
}