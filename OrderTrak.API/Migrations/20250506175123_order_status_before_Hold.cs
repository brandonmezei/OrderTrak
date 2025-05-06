using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class order_status_before_Hold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusIDBeforeHold",
                table: "ORD_Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Order_StatusIDBeforeHold",
                table: "ORD_Order",
                column: "StatusIDBeforeHold");

            migrationBuilder.AddForeignKey(
                name: "FK_ORD_Order_ORD_Status_StatusIDBeforeHold",
                table: "ORD_Order",
                column: "StatusIDBeforeHold",
                principalTable: "ORD_Status",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORD_Order_ORD_Status_StatusIDBeforeHold",
                table: "ORD_Order");

            migrationBuilder.DropIndex(
                name: "IX_ORD_Order_StatusIDBeforeHold",
                table: "ORD_Order");

            migrationBuilder.DropColumn(
                name: "StatusIDBeforeHold",
                table: "ORD_Order");
        }
    }
}
