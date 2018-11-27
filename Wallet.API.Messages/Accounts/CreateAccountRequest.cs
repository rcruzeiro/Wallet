using System;
using Newtonsoft.Json.Linq;

namespace Wallet.API.Messages.Accounts
{
    public sealed class CreateAccountRequest
    {
        public string AccountID { get; set; }
        public string LocationID { get; set; }
        public decimal InitialValue { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public JObject ExtensionAttributes { get; set; }
    }
}
