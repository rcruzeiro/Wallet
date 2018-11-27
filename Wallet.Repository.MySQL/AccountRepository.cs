using Wallet.Entities;
using Wallet.Repository.MySQL.Context;

namespace Wallet.Repository.MySQL
{
    public sealed class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(string connstring)
            : base(new ContextFactory(connstring).CreateDbContext())
        { }
    }
}
