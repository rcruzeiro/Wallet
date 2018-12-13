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
    public class AccountsController : BaseController
    {
        public AccountsController(IConfiguration configuration)
            : base(configuration)
        { }
        /// <summary>
        /// Gets an account by his ID.
        /// </summary>
        /// <returns>The account.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Account identifier.</param>
        /// <response code="200">Get request returned with sucess.</response>
        /// <response code="404">Account not found.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccountResponse), 200)]
        [ProducesResponseType(typeof(GetAccountResponse), 404)]
        [ProducesResponseType(typeof(GetAccountResponse), 500)]
        [HttpGet("{id}")]
        public ActionResult<GetAccountResponse> Get([FromHeader]string client, [FromRoute]string id)
        {
            GetAccountResponse response = new GetAccountResponse();
            string responseCode = $"GET_ACCOUNT_{client}_{id}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration, true);
                var account = factory.GetAccount(client, id);

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
        /// Creates a new giftcard.
        /// </summary>
        /// <returns>The giftcard.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Giftcard created with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateAccountResponse), 200)]
        [ProducesResponseType(typeof(CreateAccountResponse), 500)]
        [HttpPost]
        public async Task<ActionResult<CreateAccountResponse>> CreateGiftcard([FromHeader]string client, [FromBody]CreateAccountRequest request)
        {
            CreateAccountResponse response = new CreateAccountResponse();
            string responseCode = $"CREATE_GIFT_{client}";

            try
            {
                var factory = WalletFactory.Instance.GetCreateAccount(_configuration);
                var account = await factory.Create(client, null,
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
        /// Update the account.
        /// </summary>
        /// <returns>The updated account ID.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">Account updated with success.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UpdateAccountResponse), 200)]
        [ProducesResponseType(typeof(UpdateAccountResponse), 500)]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateAccountResponse>> Put([FromHeader]string client, [FromRoute]string id, [FromBody]UpdateAccountRequest request)
        {
            UpdateAccountResponse response = new UpdateAccountResponse();
            string responseCode = $"UPDATE_{client}_{id}";

            try
            {
                var factory = WalletFactory.Instance.GetAccount(_configuration, true);
                var account = await factory.UpdateGiftcard(client, id, request.CPF);
                var dto = new UpdateAccountDTO
                {
                    AccountID = account.AccountID
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
        /// Charge an giftcard.
        /// </summary>
        /// <returns>The transaction ID.</returns>
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="200">The giftcard was charged successfully.</response>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManageAccountResponse), 200)]
        [ProducesResponseType(typeof(ManageAccountResponse), 500)]
        [HttpPost("{id}/charge")]
        public async Task<ActionResult<ManageAccountResponse>> Charge([FromHeader]string client, [FromRoute]string id, [FromBody]ChargeAccountRequest request)
        {
            ManageAccountResponse response = new ManageAccountResponse();
            string responseCode = $"CHARGE_{client}_{id}";

            try
            {
                var factory = WalletFactory.Instance.GetChargeGiftcard(_configuration);
                var transaction = await factory.Charge(client, id,
                                                   request.LocationID, request.Value, request.NowExpiresOn);
                var dto = new ManageAccountDTO
                {
                    TransactionID = transaction
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
        /// <param name="client">Client identifier.</param>
        /// <param name="id">Account identifier.</param>
        /// <param name="request">Request DTO.</param>
        /// <response code="500">Server internal error. See response messages for details.</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(ManageAccountResponse), 200)]
        [ProducesResponseType(typeof(ManageAccountResponse), 500)]
        [HttpPost("{id}/consume")]
        public async Task<ActionResult<ManageAccountResponse>> ConsumeAccount([FromHeader]string client, [FromRoute]string id, [FromBody]ConsumeAccountRequest request)
        {
            ManageAccountResponse response = new ManageAccountResponse();
            string responseCode = $"CONSUME_{client}_{id}";

            try
            {
                var factory = WalletFactory.Instance.GetConsumeAccount(_configuration);
                var transaction = await factory.Consume(client, id, request.LocationID, request.Value);
                var dto = new ManageAccountDTO
                {
                    TransactionID = transaction
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
    }
}
