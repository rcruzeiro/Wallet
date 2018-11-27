using Core.Framework.Blockchain;
using MongoDB.Bson;

namespace Wallet.Objects
{
    public class WalletBlockchain : Blockchain
    {
        public ObjectId _id { get; set; }
    }
}
