using System;
using Wallet.Services.Entity;
using Wallet.Services.Task;

namespace Wallet.DI
{
    static class ServiceFactory
    {
        internal static AccountEntityService GetAccountEntityService()
        {
            try
            {
                return new AccountEntityService(DatabaseFactory.GetAccountRepository());
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static TransactionEntityService GetTransactionEntityService()
        {
            try
            {
                return new TransactionEntityService(DatabaseFactory.GetTransactionRepository());
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static CreateAccountTaskService GetCreateAccountTaskService()
        {
            try
            {
                return new CreateAccountTaskService(GetAccountEntityService(), GetTransactionEntityService());
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ChargeGiftcardTaskService GetChargeGiftcardTaskService()
        {
            try
            {
                return new ChargeGiftcardTaskService(GetAccountEntityService(), GetTransactionEntityService());
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ConsumeAccountTaskService GetConsumeAccountTaskService()
        {
            try
            {
                return new ConsumeAccountTaskService(GetAccountEntityService(), GetTransactionEntityService());
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
