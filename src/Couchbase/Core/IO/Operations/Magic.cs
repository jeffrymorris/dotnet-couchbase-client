namespace Couchbase.Core.IO.Operations
{
    internal enum Magic : byte
    {
        //Basic memcached
        Request = 0x80,
        Response = 0x81,

        //Tracing? Error Map?
        AltResponse = 0x18,

        //Synchronous Durability
        AltRequest = 0x08
    }
}
