using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class ORD_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "ORD_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ORD_Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Order_StatusID",
                table: "ORD_Order",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ORD_Order_ORD_Status_StatusID",
                table: "ORD_Order",
                column: "StatusID",
                principalTable: "ORD_Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORD_Order_ORD_Status_StatusID",
                table: "ORD_Order");

            migrationBuilder.DropTable(
                name: "ORD_Status");

            migrationBuilder.DropIndex(
                name: "IX_ORD_Order_StatusID",
                table: "ORD_Order");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "ORD_Order");
        }
    }
}
