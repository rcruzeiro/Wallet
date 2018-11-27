﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wallet.Repository.MySQL.Context;

namespace Wallet.Repository.MySQL.Migrations
{
    [DbContext(typeof(WalletContext))]
    [Migration("20181126124637_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Wallet.Entities.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountID")
                        .IsRequired()
                        .HasColumnName("accountId");

                    b.Property<int>("AccountType")
                        .HasColumnName("accountType")
                        .HasColumnType("int")
                        .HasAnnotation("description", "1=voucher;2=giftcard");

                    b.Property<decimal>("Balance")
                        .HasColumnName("balance")
                        .HasColumnType("decimal(7,2)");

                    b.Property<string>("CPF")
                        .HasColumnName("cpf");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnName("clientId");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTimeOffset>("ExpiresOn")
                        .HasColumnName("expiresOn")
                        .HasColumnType("datetime");

                    b.Property<string>("ExtensionAttributes")
                        .HasColumnName("extensionAttr");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnName("hash");

                    b.Property<decimal>("InitialValue")
                        .HasColumnName("initialValue")
                        .HasColumnType("decimal(7,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.HasIndex("CPF");

                    b.HasIndex("ClientID");

                    b.HasIndex("Hash")
                        .IsUnique();

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("Wallet.Entities.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountID")
                        .IsRequired()
                        .HasColumnName("accountId");

                    b.Property<string>("CPF")
                        .HasColumnName("cpf");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnName("clientId");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("EventType")
                        .HasColumnName("eventType")
                        .HasColumnType("int")
                        .HasAnnotation("description", "1=create;2=charge;3=consume");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnName("hash");

                    b.Property<string>("LocationID")
                        .HasColumnName("locationId");

                    b.Property<int>("OperationType")
                        .HasColumnName("operationType")
                        .HasColumnType("int")
                        .HasAnnotation("description", "1=credit;2=debit");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<decimal>("Value")
                        .HasColumnName("value")
                        .HasColumnType("decimal(7,2)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("CPF");

                    b.HasIndex("ClientID");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("Wallet.Entities.Transaction", b =>
                {
                    b.HasOne("Wallet.Entities.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountID")
                        .HasPrincipalKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}