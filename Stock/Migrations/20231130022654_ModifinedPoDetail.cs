using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Migrations
{
    /// <inheritdoc />
    public partial class ModifinedPoDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PoDetailId",
                table: "PoDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoDetails_PoDetailId",
                table: "PoDetails",
                column: "PoDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoDetails_PoDetails_PoDetailId",
                table: "PoDetails",
                column: "PoDetailId",
                principalTable: "PoDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoDetails_PoDetails_PoDetailId",
                table: "PoDetails");

            migrationBuilder.DropIndex(
                name: "IX_PoDetails_PoDetailId",
                table: "PoDetails");

            migrationBuilder.DropColumn(
                name: "PoDetailId",
                table: "PoDetails");
        }
    }
}
