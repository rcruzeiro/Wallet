using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Wallet.DTO.Transactions;

namespace Wallet.DTO.Accounts
{
    public sealed class AccountDTO
    {
        public string ClientID { get; set; }
        public string AccountID { get; set; }
        public string CPF { get; set; }
        public int AccountType { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public JObject ExtensionAttributes { get; set; }
        public List<TransactionDTO> Transactions { get; set; }

        public AccountDTO()
        {
            Transactions = new List<TransactionDTO>();
        }
    }
}
