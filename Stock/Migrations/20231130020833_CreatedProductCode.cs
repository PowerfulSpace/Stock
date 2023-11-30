using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Migrations
{
    /// <inheritdoc />
    public partial class CreatedProductCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PoDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "smallmoney", nullable: false),
                    Fob = table.Column<decimal>(type: "smallmoney", nullable: false),
                    PrcInBaseCur = table.Column<decimal>(type: "smallmoney", nullable: false),
                    PoHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoDetails_PoHeaders_PoHeaderId",
                        column: x => x.PoHeaderId,
                        principalTable: "PoHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoDetails_Products_ProductCode",
                        column: x => x.ProductCode,
                        principalTable: "Products",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoDetails_PoHeaderId",
                table: "PoDetails",
                column: "PoHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PoDetails_ProductCode",
                table: "PoDetails",
                column: "ProductCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoDetails");
        }
    }
}
