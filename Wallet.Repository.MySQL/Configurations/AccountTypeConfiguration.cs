using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallet.Entities;

namespace Wallet.Repository.MySQL.Configurations
{
    class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");
            builder.HasKey(ac => ac.ID);
            builder.Property(ac => ac.ClientID).HasColumnName("clientId").IsRequired();
            builder.Property(ac => ac.AccountID).HasColumnName("accountId").IsRequired();
            builder.Property(ac => ac.CPF).HasColumnName("cpf");
            builder.Property(ac => ac.AccountType).HasColumnName("accountType")
                   .HasAnnotation("description", "1=voucher;2=giftcard").HasColumnType("int").IsRequired();
            builder.Property(ac => ac.InitialValue).HasColumnName("initialValue").HasColumnType("decimal(7,2)").IsRequired();
            builder.Property(ac => ac.Balance).HasColumnName("balance").HasColumnType("decimal(7,2)").IsRequired();
            builder.Property(ac => ac.ExpiresOn).HasColumnName("expiresOn").HasColumnType("datetime").IsRequired();
            builder.Property(ac => ac.ExtensionAttributes).HasColumnName("extensionAttr").HasConversion(
                ea => JsonConvert.SerializeObject(ea, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                ea => JsonConvert.DeserializeObject<JObject>(ea, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            builder.Property(ac => ac.Hash).HasColumnName("hash").IsRequired();
            builder.Property(ac => ac.CreatedAt).HasColumnName("createdAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(ac => ac.UpdatedAt).HasColumnName("updatedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            //relationships
            builder.HasMany<Transaction>(ac => ac.Transactions).WithOne(tr => tr.Account)
                   .HasForeignKey(tr => tr.AccountID).HasPrincipalKey(ac => ac.AccountID)
                   .OnDelete(DeleteBehavior.Cascade);
            //UK
            builder.HasIndex(ac => ac.AccountID).IsUnique();
            builder.HasIndex(ac => ac.Hash).IsUnique();
            //index
            builder.HasIndex(ac => ac.ClientID);
            builder.HasIndex(ac => ac.CPF);
        }
    }
}
