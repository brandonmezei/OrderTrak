using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class INV_StockStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "INV_Stock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "INV_StockStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_StockStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_StatusID",
                table: "INV_Stock",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_INV_Stock_INV_StockStatus_StatusID",
                table: "INV_Stock",
                column: "StatusID",
                principalTable: "INV_StockStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_INV_Stock_INV_StockStatus_StatusID",
                table: "INV_Stock");

            migrationBuilder.DropTable(
                name: "INV_StockStatus");

            migrationBuilder.DropIndex(
                name: "IX_INV_Stock_StatusID",
                table: "INV_Stock");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "INV_Stock");
        }
    }
}
