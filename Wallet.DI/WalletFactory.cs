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

        public AccountEntityService GetAccount(IConfiguration configuration, bool read = false)
        {
            try
            {
                return ServiceFactory.GetAccountEntityService(configuration, read);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public TransactionEntityService GetTransaction(IConfiguration configuration, bool read = false)
        {
            try
            {
                return ServiceFactory.GetTransactionEntityService(configuration, read);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public CreateAccountTaskService GetCreateAccount(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetCreateAccountTaskService(configuration, false);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ChargeGiftcardTaskService GetChargeGiftcard(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetChargeGiftcardTaskService(configuration, false);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ConsumeAccountTaskService GetConsumeAccount(IConfiguration configuration)
        {
            try
            {
                return ServiceFactory.GetConsumeAccountTaskService(configuration, false);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
