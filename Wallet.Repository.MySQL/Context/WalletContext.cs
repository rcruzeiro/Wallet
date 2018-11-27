using Microsoft.EntityFrameworkCore;
using Wallet.Repository.MySQL.Configurations;

namespace Wallet.Repository.MySQL.Context
{
    sealed class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
