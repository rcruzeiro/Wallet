using System;
using Core.Framework.API.Messages;

namespace Wallet.API.Messages.Accounts
{
    public sealed class ChargeAccountRequest : BaseRequest
    {
        public string LocationID { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset NowExpiresOn { get; set; }
    }
}
