using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Wallet.Entities
{
    public class Account : BaseEntity
    {
        public string ClientID { get; set; }
        public string AccountID { get; set; }
        public string CPF { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InitialValue { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public JObject ExtensionAttributes { get; set; }
        public string Hash { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
    }
}
