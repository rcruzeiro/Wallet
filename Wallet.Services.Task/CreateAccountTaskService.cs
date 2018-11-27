using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wallet.Services.Entity;

namespace Wallet.Services.Task
{
    public sealed class CreateAccountTaskService
    {
        readonly AccountEntityService _accountES;
        readonly TransactionEntityService _transactionES;

        public CreateAccountTaskService(AccountEntityService accountES, TransactionEntityService transactionES)
        {
            _accountES = accountES;
            _transactionES = transactionES;
        }

        public async Task<Tuple<string, string>> Create(string clientID, string cpf, string accountID, string locationID, decimal initialValue, DateTimeOffset expiresOn, int accountType, JObject extensionAttributes = null)
        {
            try
            {
                var account = await _accountES.Create(clientID, cpf, accountID, initialValue, expiresOn, accountType, extensionAttributes);
                var transaction = await _transactionES.Save(clientID, cpf, account.AccountID, locationID, 1, 1, initialValue);
                return new Tuple<string, string>(account.AccountID, transaction.Hash);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
