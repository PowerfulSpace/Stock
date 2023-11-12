using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Migrations
{
    /// <inheritdoc />
    public partial class AddidSupplierModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
