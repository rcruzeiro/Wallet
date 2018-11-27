using System;
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

        public AccountEntityService GetAccount()
        {
            try
            {
                return ServiceFactory.GetAccountEntityService();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public TransactionEntityService GetTransaction()
        {
            try
            {
                return ServiceFactory.GetTransactionEntityService();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public CreateAccountTaskService GetCreateAccount()
        {
            try
            {
                return ServiceFactory.GetCreateAccountTaskService();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ChargeGiftcardTaskService GetChargeGiftcard()
        {
            try
            {
                return ServiceFactory.GetChargeGiftcardTaskService();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public ConsumeAccountTaskService GetConsumeAccount()
        {
            try
            {
                return ServiceFactory.GetConsumeAccountTaskService();
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
