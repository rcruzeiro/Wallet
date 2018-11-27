using Core.Framework.API.Messages;

namespace Wallet.API.Messages.Accounts
{
    public class UpdateAccountRequest : BaseRequest
    {
        public string CPF { get; set; }
    }
}
