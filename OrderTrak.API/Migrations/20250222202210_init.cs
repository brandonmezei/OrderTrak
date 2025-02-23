using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrak.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_ChangeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ChangeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UPL_Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UPL_PartInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PartUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStock = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_PartInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ChangeLogDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeLogId = table.Column<int>(type: "int", nullable: false),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    TicketInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ChangeLogDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_ChangeLogDetails_SYS_ChangeLog_ChangeLogId",
                        column: x => x.ChangeLogId,
                        principalTable: "SYS_ChangeLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UPL_Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDF10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UPL_Project_UPL_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "UPL_Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO_Header",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_Header", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PO_Header_UPL_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "UPL_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UPL_ProjectPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Serial = table.Column<bool>(type: "bit", nullable: false),
                    Asset = table.Column<bool>(type: "bit", nullable: false),
                    UDF1Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF2Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF3Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF4Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF5Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF6Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF7Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF8Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF9Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF10Visible = table.Column<bool>(type: "bit", nullable: false),
                    UDF2Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF3Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF4Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF5Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF6Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF7Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF8Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF9Mandatory = table.Column<bool>(type: "bit", nullable: false),
                    UDF10Mandatory = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PO_Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POHeaderID = table.Column<int>(type: "int", nullable: false),
                    ProjectPartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_Line", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PO_Line_PO_Header_POHeaderID",
                        column: x => x.POHeaderID,
                        principalTable: "PO_Header",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PO_Line_UPL_ProjectPart_ProjectPartID",
                        column: x => x.ProjectPartID,
                        principalTable: "UPL_ProjectPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PO_Header_ProjectID",
                table: "PO_Header",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Line_POHeaderID",
                table: "PO_Line",
                column: "POHeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Line_ProjectPartID",
                table: "PO_Line",
                column: "ProjectPartID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ChangeLogDetails_ChangeLogId",
                table: "SYS_ChangeLogDetails",
                column: "ChangeLogId");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_Project_CustomerID",
                table: "UPL_Project",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_ProjectPart_PartID",
                table: "UPL_ProjectPart",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_ProjectPart_ProjectID",
                table: "UPL_ProjectPart",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PO_Line");

            migrationBuilder.DropTable(
                name: "SYS_ChangeLogDetails");

            migrationBuilder.DropTable(
                name: "SYS_Users");

            migrationBuilder.DropTable(
                name: "PO_Header");

            migrationBuilder.DropTable(
                name: "UPL_ProjectPart");

            migrationBuilder.DropTable(
                name: "SYS_ChangeLog");

            migrationBuilder.DropTable(
                name: "UPL_PartInfo");

            migrationBuilder.DropTable(
                name: "UPL_Project");

            migrationBuilder.DropTable(
                name: "UPL_Customer");
        }
    }
}
