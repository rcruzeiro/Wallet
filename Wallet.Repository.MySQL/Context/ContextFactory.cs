using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wallet.Repository.MySQL.Context
{
    sealed class ContextFactory : IDesignTimeDbContextFactory<WalletContext>
    {
        readonly string _connstring;

        public ContextFactory(string connstring)
        {
            _connstring = connstring;
        }

        //constructor only used by the dotnet ef commands
        public ContextFactory()
        {
            _connstring = "Server=localhost;Port=3306;Uid=root;Pwd=secret;Database=wallet";
        }

        public WalletContext CreateDbContext(string[] args)
        {
            try
            {
                var builder = new DbContextOptionsBuilder<WalletContext>();
                builder.UseLazyLoadingProxies();
                builder.UseMySql(_connstring);
                return new WalletContext(builder.Options);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public WalletContext CreateDbContext()
        {
            try
            {
                return CreateDbContext(null);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
