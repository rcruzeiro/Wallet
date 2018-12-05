using System;
using Microsoft.Extensions.Configuration;
using Wallet.Services.Entity;
using Wallet.Services.Task;

namespace Wallet.DI
{
    static class ServiceFactory
    {
        internal static AccountEntityService GetAccountEntityService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new AccountEntityService(DatabaseFactory.GetAccountRepository(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static TransactionEntityService GetTransactionEntityService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new TransactionEntityService(DatabaseFactory.GetTransactionRepository(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static WalletBlockchainEntityService GetWalletBlockchainEntityService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new WalletBlockchainEntityService(DatabaseFactory.GetWalletBlockchainRepository(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static CreateAccountTaskService GetCreateAccountTaskService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new CreateAccountTaskService(GetAccountEntityService(configuration, read),
                                                    GetTransactionEntityService(configuration, read),
                                                    GetWalletBlockchainEntityService(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ChargeGiftcardTaskService GetChargeGiftcardTaskService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new ChargeGiftcardTaskService(GetAccountEntityService(configuration, read),
                                                     GetTransactionEntityService(configuration, read),
                                                     GetWalletBlockchainEntityService(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ConsumeAccountTaskService GetConsumeAccountTaskService(IConfiguration configuration, bool read = false)
        {
            try
            {
                return new ConsumeAccountTaskService(GetAccountEntityService(configuration, read),
                                                     GetTransactionEntityService(configuration, read),
                                                     GetWalletBlockchainEntityService(configuration, read));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
