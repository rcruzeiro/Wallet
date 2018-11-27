using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Wallet.Services.Entity;

namespace Wallet.Services.Task
{
    public sealed class ConsumeAccountTaskService
    {
        readonly AccountEntityService _accountES;
        readonly TransactionEntityService _transactionES;
        readonly WalletBlockchainEntityService _walletBlockchainES;

        public ConsumeAccountTaskService(AccountEntityService accountES,
                                         TransactionEntityService transactionES,
                                         WalletBlockchainEntityService walletBlockchainES)
        {
            _accountES = accountES;
            _transactionES = transactionES;
            _walletBlockchainES = walletBlockchainES;
        }

        public async Task<string> Consume(string clientID, string accountID, string locationID, decimal value)
        {
            try
            {
                var account = await _accountES.Consume(clientID, accountID, value);
                var transaction = await _transactionES.Save(clientID, account.CPF, account.AccountID, locationID, 2, 3, value);
                Block block = new Block(DateTimeOffset.Now, transaction);
                await _walletBlockchainES.AddBlock(block);
                return transaction.Hash;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<string>> Consume(string clientID, string cpf, string locationID, int accountType, decimal value)
        {
            try
            {
                List<string> hashes = new List<string>();
                var accounts = await _accountES.Consume(clientID, cpf, accountType, value);
                accounts.ForEach(async account =>
                {
                    var transaction = await _transactionES.Save(clientID, account.Item1.CPF, account.Item1.AccountID, locationID, 2, 3, account.Item2);
                    Block block = new Block(DateTimeOffset.Now, transaction);
                    await _walletBlockchainES.AddBlock(block);
                    hashes.Add(transaction.Hash);
                });
                return hashes;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
