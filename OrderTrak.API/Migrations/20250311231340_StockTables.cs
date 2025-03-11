using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class StockTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INV_Receipt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_Receipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "INV_Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptID = table.Column<int>(type: "int", nullable: false),
                    POLineID = table.Column<int>(type: "int", nullable: false),
                    StockGroupID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF10 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INV_Stock_INV_Receipt_ReceiptID",
                        column: x => x.ReceiptID,
                        principalTable: "INV_Receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INV_Stock_PO_Line_POLineID",
                        column: x => x.POLineID,
                        principalTable: "PO_Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INV_Stock_UPL_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "UPL_Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INV_Stock_UPL_StockGroup_StockGroupID",
                        column: x => x.StockGroupID,
                        principalTable: "UPL_StockGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_LocationID",
                table: "INV_Stock",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_POLineID",
                table: "INV_Stock",
                column: "POLineID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_ReceiptID",
                table: "INV_Stock",
                column: "ReceiptID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_StockGroupID",
                table: "INV_Stock",
                column: "StockGroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "INV_Stock");

            migrationBuilder.DropTable(
                name: "INV_Receipt");
        }
    }
}
