using System;
using System.Threading.Tasks;
using Core.Framework.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Wallet.Adapter;
using Wallet.API.Messages.Accounts;
using Wallet.DI;
using Wallet.DTO.Accounts;

namespace Wallet.API.Controllers
{
    [Route("clients/{clientID}/[controller]")]
    public class AccountController : BaseController
    {
        readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a new giftcard.
        /// </summary>
        /// <returns>The giftcard.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Giftcard created with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateAccountResponse), 200)]
        [ProducesResponseType(typeof(CreateAccountResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<CreateAccountResponse>> CreateGiftcard([FromRoute]string clientID, [FromBody]CreateAccountRequest request)
        {
            CreateAccountResponse response = new CreateAccountResponse();
            string responseCode = $"CREATE_GIFT_{clientID}";

            try
            {
                var factory = WalletFactory.Instance.GetCreateAccount(_configuration);
                var account = await factory.Create(clientID, null,
                                                   request.AccountID, request.LocationID,
                                                   request.InitialValue, request.ExpiresOn,
                                                   2, request.ExtensionAttributes);
                var dto = new NewAccountDTO
                {
                    AccountID = account.Item1,
                    TransactionID = account.Item2
                };
                response.StatusCode = "200";
                response.Data = dto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Creates a new voucher.
        /// </summary>
        /// <returns>The voucher.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Voucher created with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateAccountResponse), 200)]
        [ProducesResponseType(typeof(CreateAccountResponse), 500)]
        [HttpPost("customers/{cpf}")]
        public async Task<ActionResult<CreateAccountResponse>> CreateVoucher([FromRoute]string clientID, [FromRoute]string cpf, [FromBody]CreateAccountRequest request)
        {
            CreateAccountResponse response = new CreateAccountResponse();
            string responseCode = $"CREATE_VOUCHER_{clientID}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetCreateAccount(_configuration);
                var account = await factory.Create(clientID, cpf,
                                                   request.AccountID, request.LocationID,
                                                   request.InitialValue, request.ExpiresOn,
                                                   1, request.ExtensionAttributes);
                var dto = new NewAccountDTO
                {
                    AccountID = account.Item1,
                    TransactionID = account.Item2
                };
                response.StatusCode = "200";
                response.Data = dto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets an account by his ID.
        /// </summary>
        /// <returns>The account.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="accountID">Account identifier.</param>
        /// <response code="200">Get request returned with sucess.</response>
        /// <response code="404">Account not found.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountResponse), 200)]
        [ProducesResponseType(typeof(GetAccountResponse), 404)]
        [ProducesResponseType(typeof(GetAccountResponse), 500)]
        [HttpGet("{accountID}")]
        public ActionResult<GetAccountResponse> Get([FromRoute]string clientID, [FromRoute]string accountID)
        {
            GetAccountResponse response = new GetAccountResponse();
            string responseCode = $"GET_ACCOUNT_{clientID}_{accountID}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration);
                var account = factory.GetAccount(clientID, accountID);

                if (account == null)
                {
                    response.StatusCode = "404";
                    response.Messages.Add(ResponseMessage.Create(responseCode, "account not found."));
                    return NotFound(response);
                }

                response.StatusCode = "200";
                response.Data = account.Adapt();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets all accounts associated to the CPF.
        /// </summary>
        /// <returns>The accounts.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <response code="200">Get request returned with success.</response>
        /// <response code="404">No accounts was found for the specified CPF.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountsResponse), 200)]
        [ProducesResponseType(typeof(GetAccountsResponse), 404)]
        [ProducesResponseType(typeof(GetAccountsResponse), 500)]
        [HttpGet("customers/{cpf}")]
        public ActionResult<GetAccountsResponse> GetAccounts([FromRoute]string clientID, [FromRoute]string cpf)
        {
            GetAccountsResponse response = new GetAccountsResponse();
            string responseCode = $"GET_ACCOUNTS_{clientID}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration);
                var accounts = factory.GetAccounts(clientID, cpf);

                if (accounts.Count == 0)
                {
                    response.StatusCode = "404";
                    response.Messages.Add(ResponseMessage.Create(responseCode, "no account found for this client."));
                    return NotFound(response);
                }

                response.StatusCode = "200";
                accounts.ForEach(account =>
                                 response.Data.Add(account.Adapt()));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Gets the account type balance.
        /// </summary>
        /// <returns>The account balance.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="accountType">Account type.</param>
        /// <response code="200">The account balance returned with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountBalanceResponse), 200)]
        [ProducesResponseType(typeof(GetAccountBalanceResponse), 500)]
        [HttpGet("customers/{cpf}/balance/{accountType}")]
        public ActionResult<GetAccountBalanceResponse> GetAccountBalance([FromRoute]string clientID, [FromRoute]string cpf, [FromRoute]int accountType)
        {
            GetAccountBalanceResponse response = new GetAccountBalanceResponse();
            string responseCode = $"GET_BALANCE_{clientID}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration);
                var balance = factory.GetBalance(clientID, cpf, accountType);
                response.StatusCode = "200";
                response.Data = new AccountBalanceDTO
                {
                    Balance = string.Format("{0:N}", balance)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Update the account.
        /// </summary>
        /// <returns>The updated account ID.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="accountID">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Account updated with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountResponse), 200)]
        [ProducesResponseType(typeof(UpdateAccountResponse), 500)]
        [HttpPut("{accountID}")]
        public async Task<ActionResult<UpdateAccountResponse>> Put([FromRoute]string clientID, [FromRoute]string accountID, [FromBody]UpdateAccountRequest request)
        {
            UpdateAccountResponse response = new UpdateAccountResponse();
            string responseCode = $"UPDATE_{clientID}_{accountID}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration);
                var account = await factory.UpdateGiftcard(clientID, accountID, request.CPF);
                response.StatusCode = "200";
                response.Data = account.AccountID;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Charge an giftcard.
        /// </summary>
        /// <returns>The transaction ID.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="accountID">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">The giftcard was charged successfully.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManageAccountResponse), 200)]
        [ProducesResponseType(typeof(ManageAccountResponse), 500)]
        [HttpPost("{accountID}/charge")]
        public async Task<ActionResult<ManageAccountResponse>> Charge([FromRoute]string clientID, [FromRoute]string accountID, [FromBody]ChargeAccountRequest request)
        {
            ManageAccountResponse response = new ManageAccountResponse();
            string responseCode = $"CHARGE_{clientID}_{accountID}";

            try
            {
                var factory = WalletFactory.Instance.GetChargeGiftcard(_configuration);
                var account = await factory.Charge(clientID, accountID,
                                                   request.LocationID, request.Value, request.NowExpiresOn);
                var dto = new ManageAccountDTO
                {
                    TransactionID = account
                };
                response.StatusCode = "200";
                response.Data = dto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Consume the balance of an account.
        /// </summary>
        /// <returns>The transaction ID.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="accountID">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountResponse), 200)]
        [ProducesResponseType(typeof(UpdateAccountResponse), 500)]
        [HttpPost("{accountID}/consume")]
        public async Task<ActionResult<UpdateAccountResponse>> ConsumeAccount([FromRoute]string clientID, [FromRoute]string accountID, [FromBody]ConsumeAccountRequest request)
        {
            UpdateAccountResponse response = new UpdateAccountResponse();
            string responseCode = $"CONSUME_{clientID}_{accountID}";

            try
            {
                var factory = WalletFactory.Instance.GetConsumeAccount(_configuration);
                var account = await factory.Consume(clientID, accountID, request.LocationID, request.Value);
                response.StatusCode = "200";
                response.Data = account;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
        /// <summary>
        /// Consume the balance of an account type.
        /// </summary>
        /// <returns>All transactions IDs.</returns>
        /// <param name="clientID">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="accountType">Account Type identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountsResponse), 200)]
        [ProducesResponseType(typeof(UpdateAccountsResponse), 500)]
        [HttpPost("customers/{cpf}/consume/{accountType}")]
        public async Task<ActionResult<UpdateAccountsResponse>> ConsumeBalance([FromRoute]string clientID, [FromRoute]string cpf, [FromRoute]int accountType, [FromBody]ConsumeAccountRequest request)
        {
            UpdateAccountsResponse response = new UpdateAccountsResponse();
            string responseCode = $"CONSUME_ACCOUNT_{clientID}_{accountType}_FROM_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetConsumeAccount(_configuration);
                var account = await factory.Consume(clientID, cpf, request.LocationID, accountType, request.Value);
                response.StatusCode = "200";
                account.ForEach(trID =>
                    response.Data.Add(trID));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = "500";
                response.Messages.Add(ResponseMessage.Create(ex, responseCode));
                return StatusCode(500, response);
            }
        }
    }
}
