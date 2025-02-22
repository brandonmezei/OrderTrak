using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class SYS_User_Role_Link : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "SYS_Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_SYS_Roles_RoleID",
                table: "SYS_Users",
                column: "RoleID",
                principalTable: "SYS_Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_Users_SYS_Roles_RoleID",
                table: "SYS_Users");

            migrationBuilder.DropIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "SYS_Users");
        }
    }
}
