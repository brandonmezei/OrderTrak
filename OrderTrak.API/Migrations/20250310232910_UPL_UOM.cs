using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class UPL_UOM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPL_PartInfo_UPL_PartUOM_UOMID",
                table: "UPL_PartInfo");

            migrationBuilder.DropTable(
                name: "UPL_PartUOM");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "UPL_Location");

            migrationBuilder.AddColumn<int>(
                name: "UOMID",
                table: "UPL_Location",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UPL_UOM",
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
                    table.PrimaryKey("PK_UPL_UOM", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UPL_Location_UOMID",
                table: "UPL_Location",
                column: "UOMID");

            migrationBuilder.AddForeignKey(
                name: "FK_UPL_Location_UPL_UOM_UOMID",
                table: "UPL_Location",
                column: "UOMID",
                principalTable: "UPL_UOM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UPL_PartInfo_UPL_UOM_UOMID",
                table: "UPL_PartInfo",
                column: "UOMID",
                principalTable: "UPL_UOM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPL_Location_UPL_UOM_UOMID",
                table: "UPL_Location");

            migrationBuilder.DropForeignKey(
                name: "FK_UPL_PartInfo_UPL_UOM_UOMID",
                table: "UPL_PartInfo");

            migrationBuilder.DropTable(
                name: "UPL_UOM");

            migrationBuilder.DropIndex(
                name: "IX_UPL_Location_UOMID",
                table: "UPL_Location");

            migrationBuilder.DropColumn(
                name: "UOMID",
                table: "UPL_Location");

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "UPL_Location",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UPL_PartUOM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    UnitOfMeasurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_PartUOM", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UPL_PartInfo_UPL_PartUOM_UOMID",
                table: "UPL_PartInfo",
                column: "UOMID",
                principalTable: "UPL_PartUOM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
