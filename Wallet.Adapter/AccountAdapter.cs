using System;
using Wallet.DTO.Accounts;
using Wallet.Entities;

namespace Wallet.Adapter
{
    public static class AccountAdapter
    {
        public static AccountDTO Adapt(this Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            try
            {
                var dto = new AccountDTO
                {
                    AccountID = account.AccountID,
                    AccountType = (int)account.AccountType,
                    Balance = account.Balance,
                    ClientID = account.ClientID,
                    CPF = account.CPF,
                    ExpiresOn = account.ExpiresOn,
                    ExtensionAttributes = account.ExtensionAttributes
                };

                account.Transactions.ForEach(transaction =>
                                             dto.Transactions.Add(transaction.Adapt()));
                return dto;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
