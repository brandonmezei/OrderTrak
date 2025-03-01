using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class PartCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PO_Line_UPL_ProjectPart_ProjectPartID",
                table: "PO_Line");

            migrationBuilder.DropTable(
                name: "UPL_ProjectPart");

            migrationBuilder.RenameColumn(
                name: "ProjectPartID",
                table: "PO_Line",
                newName: "PartID");

            migrationBuilder.RenameIndex(
                name: "IX_PO_Line_ProjectPartID",
                table: "PO_Line",
                newName: "IX_PO_Line_PartID");

            migrationBuilder.AddForeignKey(
                name: "FK_PO_Line_UPL_PartInfo_PartID",
                table: "PO_Line",
                column: "PartID",
                principalTable: "UPL_PartInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PO_Line_UPL_PartInfo_PartID",
                table: "PO_Line");

            migrationBuilder.RenameColumn(
                name: "PartID",
                table: "PO_Line",
                newName: "ProjectPartID");

            migrationBuilder.RenameIndex(
                name: "IX_PO_Line_PartID",
                table: "PO_Line",
                newName: "IX_PO_Line_ProjectPartID");

            migrationBuilder.CreateTable(
                name: "UPL_ProjectPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    Asset = table.Column<bool>(type: "bit", nullable: false),
                    Serial = table.Column<bool>(type: "bit", nullable: false),
                    UDF10Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF10Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF1Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF2Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF2Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF3Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF3Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF4Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF4Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF5Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF5Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF6Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF6Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF7Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF7Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF8Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF8Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF9Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF9Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_ProjectPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UPL_ProjectPart_UPL_PartInfo_PartID",
                        column: x => x.PartID,
                        principalTable: "UPL_PartInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UPL_ProjectPart_UPL_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "UPL_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UPL_ProjectPart_PartID",
                table: "UPL_ProjectPart",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_ProjectPart_ProjectID",
                table: "UPL_ProjectPart",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_PO_Line_UPL_ProjectPart_ProjectPartID",
                table: "PO_Line",
                column: "ProjectPartID",
                principalTable: "UPL_ProjectPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
