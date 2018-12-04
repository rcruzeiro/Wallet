using System;
using Microsoft.Extensions.Configuration;
using Wallet.Repository;
using Wallet.Repository.Mongo;
using Wallet.Repository.MySQL;

namespace Wallet.DI
{
    static class DatabaseFactory
    {
        internal static IAccountRepository GetAccountRepository(IConfiguration configuration, string connName)
        {
            try
            {
                return new AccountRepository(configuration.GetConnectionString(connName));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ITransactionRepository GetTransactionRepository(IConfiguration configuration, string connName)
        {
            try
            {
                return new TransactionRepository(configuration.GetConnectionString(connName));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static IWalletBlockchainRepository GetWalletBlockchainRepository(IConfiguration configuration, string connName)
        {
            try
            {
                return new WalletBlockchainRepository(configuration.GetValue<string>(connName));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
