using System;
using Microsoft.Extensions.Configuration;
using Wallet.Services.Entity;
using Wallet.Services.Task;

namespace Wallet.DI
{
    static class ServiceFactory
    {
        internal static AccountEntityService GetAccountEntityService(IConfiguration configuration)
        {
            try
            {
                return new AccountEntityService(DatabaseFactory.GetAccountRepository(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static TransactionEntityService GetTransactionEntityService(IConfiguration configuration)
        {
            try
            {
                return new TransactionEntityService(DatabaseFactory.GetTransactionRepository(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static WalletBlockchainEntityService GetWalletBlockchainEntityService(IConfiguration configuration)
        {
            try
            {
                return new WalletBlockchainEntityService(DatabaseFactory.GetWalletBlockchainRepository(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static CreateAccountTaskService GetCreateAccountTaskService(IConfiguration configuration)
        {
            try
            {
                return new CreateAccountTaskService(GetAccountEntityService(configuration),
                                                    GetTransactionEntityService(configuration),
                                                    GetWalletBlockchainEntityService(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ChargeGiftcardTaskService GetChargeGiftcardTaskService(IConfiguration configuration)
        {
            try
            {
                return new ChargeGiftcardTaskService(GetAccountEntityService(configuration),
                                                     GetTransactionEntityService(configuration),
                                                     GetWalletBlockchainEntityService(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ConsumeAccountTaskService GetConsumeAccountTaskService(IConfiguration configuration)
        {
            try
            {
                return new ConsumeAccountTaskService(GetAccountEntityService(configuration),
                                                     GetTransactionEntityService(configuration),
                                                     GetWalletBlockchainEntityService(configuration));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
