using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallet.Repository.MySQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    clientId = table.Column<string>(nullable: false),
                    accountId = table.Column<string>(nullable: false),
                    cpf = table.Column<string>(nullable: true),
                    accountType = table.Column<int>(type: "int", nullable: false),
                    initialValue = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    expiresOn = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    extensionAttr = table.Column<string>(nullable: true),
                    hash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.ID);
                    table.UniqueConstraint("AK_accounts_accountId", x => x.accountId);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    clientId = table.Column<string>(nullable: false),
                    cpf = table.Column<string>(nullable: true),
                    accountId = table.Column<string>(nullable: false),
                    locationId = table.Column<string>(nullable: true),
                    operationType = table.Column<int>(type: "int", nullable: false),
                    eventType = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    hash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "accounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_accountId",
                table: "accounts",
                column: "accountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_cpf",
                table: "accounts",
                column: "cpf");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_clientId",
                table: "accounts",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_hash",
                table: "accounts",
                column: "hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_accountId",
                table: "transactions",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_cpf",
                table: "transactions",
                column: "cpf");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_clientId",
                table: "transactions",
                column: "clientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
