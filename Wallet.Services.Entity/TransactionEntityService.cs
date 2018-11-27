using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wallet.Entities;
using Wallet.Repository;

namespace Wallet.Services.Entity
{
    public sealed class TransactionEntityService
    {
        readonly ITransactionRepository _repository;

        public TransactionEntityService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        ~TransactionEntityService()
        {
            _repository.Dispose();
        }

        public async Task<Transaction> Save(string clientID, string cpf, string accountID, string locationID, int operationType, int eventType, decimal value)
        {
            try
            {
                Transaction transaction = new Transaction
                {
                    ClientID = clientID,
                    CPF = cpf,
                    AccountID = accountID,
                    LocationID = locationID,
                    OperationType = (OperationType)operationType,
                    EventType = (EventType)eventType,
                    Value = value
                };

                transaction.Hash = CalculateHash(transaction);
                await _repository.AddAsync(transaction);
                await _repository.SaveAsync();
                return transaction;
            }
            catch (Exception ex)
            { throw ex; }
        }

        string CalculateHash(Transaction transaction)
        {
            try
            {
                SHA256 sHA256 = SHA256.Create();
                byte[] inputBytes = Encoding.ASCII.GetBytes(
                    $"{transaction.ID}-{transaction.ClientID}-{transaction.CPF ?? ""}-{transaction.AccountID}-{transaction.LocationID}-{transaction.EventType}-{string.Format("{0:0.##}", transaction.Value)}");
                byte[] outputBytes = sHA256.ComputeHash(inputBytes);
                return Convert.ToBase64String(outputBytes);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
