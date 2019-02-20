namespace Couchbase.Core.IO.Operations
{
    public enum OpCode : byte
    {
        #region Basic and Extended Memcached

        Get = 0x00,
        Set = 0x01,
        Add = 0x02,
        Replace = 0x03,
        Delete = 0x04,
        Increment = 0x05,
        Decrement = 0x06,
        Quit = 0x07,
        Flush = 0x08,
        GetQ = 0x09,
        NoOp = 0x0A,
        Version = 0x0B,
        GetK = 0x0C,

        // ReSharper disable once InconsistentNaming
        GetKQ = 0x0D,

        Append = 0x0E,
        Prepend = 0x0F,
        Stat = 0x10,
        SetQ = 0x11,
        AddQ = 0x12,
        ReplaceQ = 0x13,
        DeleteQ = 0x14,
        IncrementQ = 0x15,
        DecrementQ = 0x16,
        QuitQ = 0x17,
        FlushQ = 0x18,
        AppendQ = 0x19,
        PrependQ = 0x1A,

        Touch = 0x1c,
        GAT = 0x1d,
        GATQ = 0x1e,

        //couchbase only
        GetL = 0x94,
        Unlock = 0x95,

        #endregion

        #region SASL & Authentication

        SaslList = 0x20,
        SaslStart = 0x21,
        SaslStep = 0x22,

        // used with RBAC to verify credentials with username / password
        SelectBucket = 0x89,

        #endregion

        #region Management & Negoiation

        GetClusterConfig = 0xb5,

        /// <summary>
        ///     You say goodbye and I say Hello. Hello, hello.
        /// </summary>
        Helo = 0x1f,

        // request a server error map
        GetErrorMap = 0xfe,

        #endregion

        #region Replica Reads

        //"Dirty" reads
        ReplicaRead = 0x83,

        #endregion

        #region Durability

        //Enhanced durability
        ObserveSeqNo = 0x91,

        //Durability constraints
        Observe = 0x92,

        //Must be sent to allow the server to accept requests with flexible frame
        //extras, The request magic for new requests is 0x08.
        AltRequestSupport = 0x10,

        //Must be sent to be able to specify durability requirements for mutations.
        SyncReplication = 0x11,

        #endregion
   
        #region  Sub-Document

        SubGet = 0xc5,
        SubExist = 0xc6,
        SubDictAdd = 0xc7,
        SubDictUpsert = 0xc8,
        SubDelete = 0xc9,
        SubReplace = 0xca,
        SubArrayPushLast = 0xcb,
        SubArrayPushFirst = 0xcc,
        SubArrayInsert = 0xcd,
        SubArrayAddUnique = 0xce,
        SubCounter = 0xcf,
        MultiLookup = 0xd0,
        SubMultiMutation = 0xd1,
        SubGetCount = 0xd2,

        #endregion

        #region Collections

        //the collections manifest
        GetCollectionsManifest = 0xBA,

        //Get the CID by name
        CollectionsGetId = 0xbb

        #endregion
    }
}