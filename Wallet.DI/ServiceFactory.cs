using System;
using Microsoft.Extensions.Configuration;
using Wallet.Services.Entity;
using Wallet.Services.Task;

namespace Wallet.DI
{
    static class ServiceFactory
    {
        internal static AccountEntityService GetAccountEntityService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new AccountEntityService(DatabaseFactory.GetAccountRepository(configuration, connNames[0]));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static TransactionEntityService GetTransactionEntityService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new TransactionEntityService(DatabaseFactory.GetTransactionRepository(configuration, connNames[0]));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static WalletBlockchainEntityService GetWalletBlockchainEntityService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new WalletBlockchainEntityService(DatabaseFactory.GetWalletBlockchainRepository(configuration, connNames[0]));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static CreateAccountTaskService GetCreateAccountTaskService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new CreateAccountTaskService(GetAccountEntityService(configuration, connNames[0]),
                                                    GetTransactionEntityService(configuration, connNames[0]),
                                                    GetWalletBlockchainEntityService(configuration, connNames[1]));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ChargeGiftcardTaskService GetChargeGiftcardTaskService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new ChargeGiftcardTaskService(GetAccountEntityService(configuration, connNames[0]),
                                                     GetTransactionEntityService(configuration, connNames[0]),
                                                     GetWalletBlockchainEntityService(configuration, connNames[1]));
            }
            catch (Exception ex)
            { throw ex; }
        }

        internal static ConsumeAccountTaskService GetConsumeAccountTaskService(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return new ConsumeAccountTaskService(GetAccountEntityService(configuration, connNames[0]),
                                                     GetTransactionEntityService(configuration, connNames[0]),
                                                     GetWalletBlockchainEntityService(configuration, connNames[1]));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
