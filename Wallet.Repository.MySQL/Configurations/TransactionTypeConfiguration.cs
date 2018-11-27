using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Entities;

namespace Wallet.Repository.MySQL.Configurations
{
    class TransactionTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");
            builder.HasKey(tr => tr.ID);
            builder.Property(tr => tr.ClientID).HasColumnName("clientId").IsRequired();
            builder.Property(tr => tr.CPF).HasColumnName("cpf");
            builder.Property(tr => tr.AccountID).HasColumnName("accountId").IsRequired();
            builder.Property(tr => tr.LocationID).HasColumnName("locationId");
            builder.Property(tr => tr.OperationType).HasColumnName("operationType")
                   .HasAnnotation("description", "1=credit;2=debit").HasColumnType("int").IsRequired();
            builder.Property(tr => tr.EventType).HasColumnName("eventType")
                   .HasAnnotation("description", "1=create;2=charge;3=consume").HasColumnType("int").IsRequired();
            builder.Property(tr => tr.Value).HasColumnName("value").HasColumnType("decimal(7,2)").IsRequired();
            builder.Property(tr => tr.Hash).HasColumnName("hash").IsRequired();
            builder.Property(tr => tr.CreatedAt).HasColumnName("createdAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(tr => tr.UpdatedAt).HasColumnName("updatedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            //relationships
            builder.HasOne<Account>(tr => tr.Account).WithMany(ac => ac.Transactions)
                   .HasForeignKey(tr => tr.AccountID).HasPrincipalKey(ac => ac.AccountID);
            //index
            builder.HasIndex(tr => tr.ClientID);
            builder.HasIndex(tr => tr.CPF);
            builder.HasIndex(tr => tr.AccountID);
        }
    }
}
