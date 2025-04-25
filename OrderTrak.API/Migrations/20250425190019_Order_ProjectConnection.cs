using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class Order_ProjectConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "ORD_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Order_ProjectID",
                table: "ORD_Order",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_ORD_Order_UPL_Project_ProjectID",
                table: "ORD_Order",
                column: "ProjectID",
                principalTable: "UPL_Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORD_Order_UPL_Project_ProjectID",
                table: "ORD_Order");

            migrationBuilder.DropIndex(
                name: "IX_ORD_Order_ProjectID",
                table: "ORD_Order");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "ORD_Order");
        }
    }
}
