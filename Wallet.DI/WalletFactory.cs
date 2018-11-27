using System;
using Microsoft.Extensions.Configuration;
using Wallet.Services.Entity;
using Wallet.Services.Task;

namespace Wallet.DI
{
    public sealed class WalletFactory
    {
        static WalletFactory _instance;

        public static WalletFactory Instance
        { get { return _instance ?? (_instance = new WalletFactory()); } }

        WalletFactory()
        { }

        public AccountEntityService GetAccount(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetAccountEntityService(configuration);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public TransactionEntityService GetTransaction(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetTransactionEntityService(configuration);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public CreateAccountTaskService GetCreateAccount(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetCreateAccountTaskService(configuration);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ChargeGiftcardTaskService GetChargeGiftcard(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetChargeGiftcardTaskService(configuration);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ConsumeAccountTaskService GetConsumeAccount(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetConsumeAccountTaskService(configuration);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
