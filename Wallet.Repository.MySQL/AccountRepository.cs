using Wallet.Entities;
using Wallet.Repository.MySQL.Context;

namespace Wallet.Repository.MySQL
{
    public sealed class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(string connString)
            : base(new ContextFactory(connString).CreateDbContext())
        { }
    }
}
