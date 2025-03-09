using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class StockGroupCommon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "UPL_StockGroup",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateName",
                table: "UPL_StockGroup",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "FormID",
                table: "UPL_StockGroup",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "UPL_StockGroup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "UPL_StockGroup",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateName",
                table: "UPL_StockGroup",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "UPL_StockGroup");

            migrationBuilder.DropColumn(
                name: "CreateName",
                table: "UPL_StockGroup");

            migrationBuilder.DropColumn(
                name: "FormID",
                table: "UPL_StockGroup");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "UPL_StockGroup");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "UPL_StockGroup");

            migrationBuilder.DropColumn(
                name: "UpdateName",
                table: "UPL_StockGroup");
        }
    }
}
