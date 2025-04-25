using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class ProjectUpdate_OrderUDF_Stakeholder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderUDF1",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF10",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF2",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF3",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF4",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF5",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF6",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF7",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF8",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUDF9",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StakeHolderEmail",
                table: "UPL_Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderUDF1",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF10",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF2",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF3",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF4",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF5",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF6",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF7",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF8",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "OrderUDF9",
                table: "UPL_Project");

            migrationBuilder.DropColumn(
                name: "StakeHolderEmail",
                table: "UPL_Project");
        }
    }
}
