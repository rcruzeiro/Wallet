using Core.Framework.API.Messages;

namespace Wallet.API.Messages.Accounts
{
    public sealed class ConsumeAccountRequest : BaseRequest
    {
        public string LocationID { get; set; }
        public decimal Value { get; set; }
    }
}
