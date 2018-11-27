using Core.Framework.Repository;
using Wallet.Entities;

namespace Wallet.Repository
{
    public interface ITransactionRepository : IRepositoryAsync<Transaction>
    { }
}
