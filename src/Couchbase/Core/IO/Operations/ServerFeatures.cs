
namespace Couchbase.Core.IO.Operations
{  
    /// <summary>
    /// Features that they client negotiate on a per connection basis.
    /// </summary>
    internal enum ServerFeatures : short
    {
        /// <summary>
        /// For custom data types
        /// </summary>
        Datatype = 0x01,

        /// <summary>
        /// Enable TCP nodelay
        /// </summary>
        TcpNoDelay = 0x03,

        /// <summary>
        /// Return the sequence number with every mutation
        /// </summary>
        MutationSeqno = 0x04,

        /// <summary>
        /// Disable TCP nodelay
        /// </summary>
        TcpDelay = 0x05,

        /// <summary>
        /// Perform subdocument operations on document attributes
        /// </summary>
        SubdocXAttributes = 0x06,

        /// <summary>
        /// Return extended error information for the client to use in K/V Error Mapping. Implies the client
        /// will request that information from the server to use in mapping error attributes and classes.
        /// </summary>
        XError = 0x07,

        /// <summary>
        /// Indicates if the cluster supports RBAC and if a Select_Bucket operation should
        /// be executed when opening a bucket.
        /// </summary>
        SelectBucket = 0x08,

        /// <summary>
        /// Retrieve the Server Duration of the operation. This enables the server to return respondes
        /// with magic <see cref="Magic.AltResponse"/>.
        /// </summary>
        ServerDuration = 0x0f,

        Collections = 0x09


    }
}
