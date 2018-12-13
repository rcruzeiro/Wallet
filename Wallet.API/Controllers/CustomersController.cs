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
    [Route("[controller]")]
    public class CustomersController : BaseController
    {
        public CustomersController(IConfiguration configuration)
            : base(configuration)
        { }
        /// <summary>
        /// Gets all accounts associated to the CPF.
        /// </summary>
        /// <returns>The accounts.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <response code="200">Get request returned with success.</response>
        /// <response code="404">No accounts was found for the specified CPF.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountsResponse), 200)]
        [ProducesResponseType(typeof(GetAccountsResponse), 404)]
        [ProducesResponseType(typeof(GetAccountsResponse), 500)]
        [HttpGet("{cpf}")]
        public ActionResult<GetAccountsResponse> GetAccounts([FromHeader]string client, [FromRoute]string cpf)
        {
            GetAccountsResponse response = new GetAccountsResponse();
            string responseCode = $"GET_ACCOUNTS_{client}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration, true);
                var accounts = factory.GetAccounts(client, cpf);

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
        /// <param name="client">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="accountType">Account type.</param>
        /// <response code="200">The account balance returned with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountBalanceResponse), 200)]
        [ProducesResponseType(typeof(GetAccountBalanceResponse), 500)]
        [HttpGet("{cpf}/balance/{accountType}")]
        public ActionResult<GetAccountBalanceResponse> GetAccountBalance([FromHeader]string client, [FromRoute]string cpf, [FromRoute]int accountType)
        {
            GetAccountBalanceResponse response = new GetAccountBalanceResponse();
            string responseCode = $"GET_BALANCE_{client}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration, true);
                var balance = factory.GetBalance(client, cpf, accountType);
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
        /// Creates a new voucher.
        /// </summary>
        /// <returns>The voucher.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Voucher created with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateAccountResponse), 200)]
        [ProducesResponseType(typeof(CreateAccountResponse), 500)]
        [HttpPost("{cpf}")]
        public async Task<ActionResult<CreateAccountResponse>> CreateVoucher([FromHeader]string client, [FromRoute]string cpf, [FromBody]CreateAccountRequest request)
        {
            CreateAccountResponse response = new CreateAccountResponse();
            string responseCode = $"CREATE_VOUCHER_{client}_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetCreateAccount(_configuration);
                var account = await factory.Create(client, cpf,
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
        /// Consume the balance of an account type.
        /// </summary>
        /// <returns>All transactions IDs.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="cpf">Customer CPF.</param>
        /// <param name="accountType">Account Type identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManageAccountsResponse), 200)]
        [ProducesResponseType(typeof(ManageAccountsResponse), 500)]
        [HttpPost("{cpf}/consume/{accountType}")]
        public async Task<ActionResult<ManageAccountsResponse>> ConsumeBalance([FromHeader]string client, [FromRoute]string cpf, [FromRoute]int accountType, [FromBody]ConsumeAccountRequest request)
        {
            ManageAccountsResponse response = new ManageAccountsResponse();
            string responseCode = $"CONSUME_ACCOUNT_{client}_{accountType}_FROM_{cpf}";

            try
            {
                var factory = WalletFactory.Instance.GetConsumeAccount(_configuration);
                var transactions = await factory.Consume(client, cpf, request.LocationID, accountType, request.Value);
                response.StatusCode = "200";
                transactions.ForEach(transaction =>
                    response.Data.Add(new ManageAccountDTO
                    {
                        TransactionID = transaction
                    }));
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
