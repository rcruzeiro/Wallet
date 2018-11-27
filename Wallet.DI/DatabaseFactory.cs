using System;
using Wallet.Repository;
using Wallet.Repository.MySQL;

namespace Wallet.DI
{
    static class DatabaseFactory
    {
        internal static IAccountRepository GetAccountRepository()
        {
            try
            {
                return new AccountRepository("Default");
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ITransactionRepository GetTransactionRepository()
        {
            try
            {
                return new TransactionRepository("Default");
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
