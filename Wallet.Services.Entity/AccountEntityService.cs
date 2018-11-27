using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wallet.Entities;
using Wallet.Repository;

namespace Wallet.Services.Entity
{
    public sealed class AccountEntityService
    {
        readonly IAccountRepository _repository;

        public AccountEntityService(IAccountRepository repository)
        {
            _repository = repository;
        }

        ~AccountEntityService()
        {
            _repository.Dispose();
        }

        public List<Account> GetAccounts(string clientID, string cpf)
        {
            try
            {
                return _repository.Get(ac => ac.ClientID == clientID && ac.CPF == cpf &&
                                             ac.ExpiresOn > DateTimeOffset.Now &&
                                             ac.Balance > 0)
                                  .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public List<Account> GetAccounts(string clientID, string cpf, int accountType)
        {
            try
            {
                return _repository.Get(ac => ac.ClientID == clientID && ac.CPF == cpf &&
                                             ac.AccountType == (AccountType)accountType &&
                                             ac.ExpiresOn > DateTimeOffset.Now &&
                                             ac.Balance > 0)
                                  .ToList();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Account GetAccount(string clientID, string accountID)
        {
            try
            {
                return _repository.Get(ac => ac.ClientID == clientID && ac.AccountID == accountID)
                                  .SingleOrDefault();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public decimal GetBalance(string clientID, string cpf, int accountType)
        {
            try
            {
                return GetAccounts(clientID, cpf, accountType).Sum(ac => ac.Balance);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<Account> Create(string clientID, string cpf, string accountID, decimal initialValue, DateTimeOffset expiresOn, int accountType, JObject extensionAttributes)
        {
            try
            {
                if (accountType == (int)AccountType.Voucher && string.IsNullOrEmpty(cpf))
                    throw new InvalidOperationException("the CPF field cannot be blank to create a voucher account type.");

                Account account = new Account
                {
                    ClientID = clientID,
                    CPF = cpf,
                    InitialValue = initialValue,
                    Balance = initialValue,
                    ExpiresOn = expiresOn,
                    AccountType = (AccountType)accountType,
                    ExtensionAttributes = extensionAttributes
                };
                account.AccountID = accountID ?? GetAccountID(account);
                account.Hash = CalculateHash(account);
                await _repository.AddAsync(account);
                await _repository.SaveAsync();
                return account;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<Account> UpdateGiftcard(string clientID, string accountID, string cpf)
        {
            try
            {
                var account = GetAccount(clientID, accountID);

                if (account == null)
                    throw new ArgumentNullException(nameof(account), "account not found.");

                if (account.AccountType != AccountType.Giftcard)
                    throw new InvalidOperationException("this is not a valid gift card (maybe it is a voucher?)");

                if (!string.IsNullOrEmpty(account.CPF))
                    throw new InvalidOperationException("this gift card already has a CPF associated to it.");

                account.CPF = cpf;
                _repository.Update(account);
                await _repository.SaveAsync();
                return account;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<Account> ChargeGiftcard(string clientID, string accountID, decimal value, DateTimeOffset? nowExpiresOn = null)
        {
            try
            {
                var account = GetAccount(clientID, accountID);

                if (account == null)
                    throw new ArgumentNullException(nameof(account), "account not found.");

                if (account.AccountType != AccountType.Giftcard)
                    throw new InvalidOperationException("it is not possible to charge an account type other than a gift card.");

                if (string.IsNullOrEmpty(account.CPF))
                    throw new InvalidOperationException("it is not possible to charge a gift card with no CPF associated to it.");

                if (nowExpiresOn != null && nowExpiresOn != default(DateTimeOffset))
                    account.ExpiresOn = nowExpiresOn.Value;

                account.Balance += value;
                _repository.Update(account);
                await _repository.SaveAsync();
                return account;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<Account> Consume(string clientID, string accountID, decimal value)
        {
            try
            {
                var account = GetAccount(clientID, accountID);

                if (account == null)
                    throw new ArgumentNullException(nameof(account), "account not found.");

                if (value > account.Balance)
                    throw new InvalidOperationException(
                        $"the given value ({string.Format("{0:C}", value)}) is greater than the given account balance ({string.Format("{0:C}", account.Balance)}).");

                account.Balance -= value;
                _repository.Update(account);
                await _repository.SaveAsync();
                return account;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<List<Tuple<Account, decimal>>> Consume(string clientID, string cpf, int accountType, decimal value)
        {
            try
            {
                List<Tuple<Account, decimal>> rebate = new List<Tuple<Account, decimal>>();
                var accounts = GetAccounts(clientID, cpf, accountType);

                if (accounts.Count == 0)
                    throw new ArgumentOutOfRangeException(nameof(accounts), "this client has no valid account for this operation.");

                var balance = accounts.Sum(ac => ac.Balance);

                if (value > balance)
                    throw new InvalidOperationException(
                        $"the given value ({string.Format("{0:C}", value)}) is greater than the given account balance ({string.Format("{0:C}", balance)}).");

                accounts.OrderBy(ac => ac.ExpiresOn)
                        .ToList()
                        .ForEach(account =>
                        {
                            if (value > 0)
                            {
                                decimal rebateValue = value;
                                decimal residual = account.Balance - value;

                                if (residual > 0)
                                {
                                    account.Balance -= value;
                                    value = 0;
                                }
                                else
                                {
                                    rebateValue = account.Balance;
                                    value -= account.Balance;
                                    account.Balance = 0;
                                }

                                _repository.Update(account);
                                rebate.Add(new Tuple<Account, decimal>(account, rebateValue));
                            }
                        });

                await _repository.SaveAsync();
                return rebate;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool IsValid(string clientID, string accountID)
        {
            try
            {
                var account = GetAccount(clientID, accountID);

                if (account == null)
                    throw new ArgumentNullException(nameof(account), "account not found.");

                return account.Hash == CalculateHash(account);
            }
            catch (Exception ex)
            { throw ex; }
        }

        string GetAccountID(Account account)
        {
            try
            {
                string cpf = !string.IsNullOrEmpty(account.CPF)
                                    ? account.CPF.Substring(0, 4) : "0000";
                string guid = new Random().Next(1, 999999999).ToString("D9");
                string checker = new Random().Next(10, 99).ToString();
                return $"{DateTimeOffset.Now.ToString("yyyyMMdd")}{cpf}{guid}-{checker}";
            }
            catch (Exception ex)
            { throw ex; }
        }

        string CalculateHash(Account account)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                byte[] input = Encoding.ASCII.GetBytes(
                    $"{account.ClientID}-{account.AccountID}-{string.Format("{0:0.##}", account.InitialValue)}-{account.ExpiresOn.ToString("yy-dd-MM")}");
                byte[] output = sHA256.ComputeHash(input);
                return Convert.ToBase64String(output);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
