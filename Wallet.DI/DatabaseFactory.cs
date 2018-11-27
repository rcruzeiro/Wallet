using System;
using Microsoft.Extensions.Configuration;
using Wallet.Repository;
using Wallet.Repository.Mongo;
using Wallet.Repository.MySQL;

namespace Wallet.DI
{
    static class DatabaseFactory
    {
        static readonly string _connName =
            Environment.GetEnvironmentVariable("DB_CONNECTION_NAME");

        internal static IAccountRepository GetAccountRepository(IConfiguration configuration)
        {
            try
            {
                return new AccountRepository(configuration.GetConnectionString(_connName));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ITransactionRepository GetTransactionRepository(IConfiguration configuration)
        {
            try
            {
                return new TransactionRepository(configuration.GetConnectionString(_connName));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static IWalletBlockchainRepository GetWalletBlockchainRepository(IConfiguration configuration)
        {
            try
            {
                return new WalletBlockchainRepository(configuration.GetValue<string>("MongoConnection"));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
