using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Migrations
{
    /// <inheritdoc />
    public partial class AddSelfForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeCurrencyId",
                table: "Currencies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_ExchangeCurrencyId",
                table: "Currencies",
                column: "ExchangeCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Currencies_ExchangeCurrencyId",
                table: "Currencies",
                column: "ExchangeCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Currencies_ExchangeCurrencyId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_ExchangeCurrencyId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "Currencies");
        }
    }
}
