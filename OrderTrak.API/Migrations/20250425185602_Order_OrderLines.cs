using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class Order_OrderLines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORD_Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestedShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StakeHolderEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderUDF10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ORD_Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    POLineID = table.Column<int>(type: "int", nullable: false),
                    StockGroupID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_Line", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ORD_Line_ORD_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "ORD_Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORD_Line_PO_Line_POLineID",
                        column: x => x.POLineID,
                        principalTable: "PO_Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORD_Line_UPL_PartInfo_PartID",
                        column: x => x.PartID,
                        principalTable: "UPL_PartInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORD_Line_UPL_StockGroup_StockGroupID",
                        column: x => x.StockGroupID,
                        principalTable: "UPL_StockGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_OrderID",
                table: "ORD_Line",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_PartID",
                table: "ORD_Line",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_POLineID",
                table: "ORD_Line",
                column: "POLineID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_StockGroupID",
                table: "ORD_Line",
                column: "StockGroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORD_Line");

            migrationBuilder.DropTable(
                name: "ORD_Order");
        }
    }
}
