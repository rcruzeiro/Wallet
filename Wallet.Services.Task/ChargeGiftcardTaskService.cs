using System;
using System.Threading.Tasks;
using Wallet.Services.Entity;

namespace Wallet.Services.Task
{
    public sealed class ChargeGiftcardTaskService
    {
        readonly AccountEntityService _accountES;
        readonly TransactionEntityService _transactionES;

        public ChargeGiftcardTaskService(AccountEntityService accountES, TransactionEntityService transactionES)
        {
            _accountES = accountES;
            _transactionES = transactionES;
        }

        public async Task<string> Charge(string clientID, string accountID, string locationID, decimal value, DateTimeOffset? nowExpiresOn = null)
        {
            try
            {
                var account = await _accountES.ChargeGiftcard(clientID, accountID, value, nowExpiresOn);
                var transaction = await _transactionES.Save(clientID, account.CPF, account.AccountID, locationID, 1, 2, value);
                return transaction.Hash;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
