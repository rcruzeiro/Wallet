using System;
using Wallet.DTO.Transactions;
using Wallet.Entities;

namespace Wallet.Adapter
{
    public static class TransactionAdapter
    {
        public static TransactionDTO Adapt(this Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            try
            {
                return new TransactionDTO
                {
                    EventType = (int)transaction.EventType,
                    LocationID = transaction.LocationID,
                    OperationType = (int)transaction.OperationType,
                    Value = transaction.Value,
                    PlacedAt = transaction.CreatedAt ?? transaction.CreatedAt.Value,
                    TransactionID = transaction.Hash
                };
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
