using System.Collections.Generic;
using Core.Framework.API.Messages;
using Wallet.DTO.Accounts;

namespace Wallet.API.Messages.Accounts
{
    public sealed class GetAccountsResponse : BaseResponse<List<AccountDTO>>
    {
        public GetAccountsResponse()
        {
            Data = new List<AccountDTO>();
        }
    }
}
