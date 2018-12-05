using System;
using Microsoft.Extensions.Configuration;
using Wallet.Repository;
using Wallet.Repository.Mongo;
using Wallet.Repository.MySQL;

namespace Wallet.DI
{
    static class DatabaseFactory
    {
        static readonly string _writeConnection =
            Environment.GetEnvironmentVariable("DB_CONNECTION_DEFAULT_NAME");
        static readonly string _readConnection =
            Environment.GetEnvironmentVariable("DB_CONNECTION_READ_NAME");
        static readonly string _mongoConnection =
            Environment.GetEnvironmentVariable("MONGO_CONNECTION");

        internal static IAccountRepository GetAccountRepository(IConfiguration configuration, bool read = false)
        {
            try
            {
                string connstring = read ? configuration.GetConnectionString(_readConnection) :
                    configuration.GetConnectionString(_writeConnection);

                return new AccountRepository(connstring);
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ITransactionRepository GetTransactionRepository(IConfiguration configuration, bool read = false)
        {
            try
            {
                string connstring = read ? configuration.GetConnectionString(_readConnection) :
                    configuration.GetConnectionString(_writeConnection);

                return new TransactionRepository(connstring);
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static IWalletBlockchainRepository GetWalletBlockchainRepository(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new WalletBlockchainRepository(
                    configuration.GetConnectionString(_mongoConnection));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
