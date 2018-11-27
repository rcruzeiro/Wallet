using System;
using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Newtonsoft.Json.Linq;
using Wallet.Services.Entity;

namespace Wallet.Services.Task
{
    public sealed class CreateAccountTaskService
    {
        readonly AccountEntityService _accountES;
        readonly TransactionEntityService _transactionES;
        readonly WalletBlockchainEntityService _walletBlockchainES;

        public CreateAccountTaskService(AccountEntityService accountES,
                                        TransactionEntityService transactionES,
                                        WalletBlockchainEntityService walletBlockchainES)
        {
            _accountES = accountES;
            _transactionES = transactionES;
            _walletBlockchainES = walletBlockchainES;
        }

        public async Task<Tuple<string, string>> Create(string clientID, string cpf, string accountID, string locationID, decimal initialValue, DateTimeOffset expiresOn, int accountType, JObject extensionAttributes = null)
        {
            try
            {
                var account = await _accountES.Create(clientID, cpf, accountID, initialValue, expiresOn, accountType, extensionAttributes);
                var transaction = await _transactionES.Save(clientID, cpf, account.AccountID, locationID, 1, 1, initialValue);
                Block block = new Block(DateTimeOffset.Now, transaction);
                await _walletBlockchainES.AddBlock(block);
                return new Tuple<string, string>(account.AccountID, transaction.Hash);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
