using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class UPL_PartUOM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartUnit",
                table: "UPL_PartInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "Depth",
                table: "UPL_PartInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "UPL_PartInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UOMID",
                table: "UPL_PartInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "UPL_PartInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UPL_PartUOM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitOfMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_PartUOM", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UPL_PartInfo_UOMID",
                table: "UPL_PartInfo",
                column: "UOMID");

            migrationBuilder.AddForeignKey(
                name: "FK_UPL_PartInfo_UPL_PartUOM_UOMID",
                table: "UPL_PartInfo",
                column: "UOMID",
                principalTable: "UPL_PartUOM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPL_PartInfo_UPL_PartUOM_UOMID",
                table: "UPL_PartInfo");

            migrationBuilder.DropTable(
                name: "UPL_PartUOM");

            migrationBuilder.DropIndex(
                name: "IX_UPL_PartInfo_UOMID",
                table: "UPL_PartInfo");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "UPL_PartInfo");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "UPL_PartInfo");

            migrationBuilder.DropColumn(
                name: "UOMID",
                table: "UPL_PartInfo");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "UPL_PartInfo");

            migrationBuilder.AddColumn<string>(
                name: "PartUnit",
                table: "UPL_PartInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
