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

        public AccountEntityService GetAccount(IConfiguration configuration, string connName)
        {
            try
            {
                return ServiceFactory.GetAccountEntityService(configuration, connName);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public TransactionEntityService GetTransaction(IConfiguration configuration, string connName)
        {
            try
            {
                return ServiceFactory.GetTransactionEntityService(configuration, connName);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public CreateAccountTaskService GetCreateAccount(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return ServiceFactory.GetCreateAccountTaskService(configuration, connNames);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ChargeGiftcardTaskService GetChargeGiftcard(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return ServiceFactory.GetChargeGiftcardTaskService(configuration, connNames);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ConsumeAccountTaskService GetConsumeAccount(IConfiguration configuration, params string[] connNames)
        {
            try
            {
                return ServiceFactory.GetConsumeAccountTaskService(configuration, connNames);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
