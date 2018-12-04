using System.Collections.Generic;
using Core.Framework.API.Messages;
using Wallet.DTO.Accounts;

namespace Wallet.API.Messages.Accounts
{
    public class ManageAccountsResponse : BaseResponse<List<ManageAccountDTO>>
    {
        public ManageAccountsResponse()
        {
            Data = new List<ManageAccountDTO>();
        }
    }
}
