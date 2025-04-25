using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class ORD_PickList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORD_PickList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineID = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_PickList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ORD_PickList_INV_Stock_StockID",
                        column: x => x.StockID,
                        principalTable: "INV_Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORD_PickList_ORD_Line_LineID",
                        column: x => x.LineID,
                        principalTable: "ORD_Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORD_PickList_LineID",
                table: "ORD_PickList",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_PickList_StockID",
                table: "ORD_PickList",
                column: "StockID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORD_PickList");
        }
    }
}
