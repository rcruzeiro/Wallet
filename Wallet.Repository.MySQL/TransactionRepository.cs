using Wallet.Entities;
using Wallet.Repository.MySQL.Context;

namespace Wallet.Repository.MySQL
{
    public sealed class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(string connString)
            : base(new ContextFactory(connString).CreateDbContext())
        { }
    }
}
