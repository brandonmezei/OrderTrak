using System;
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
                name: "INV_StockStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_StockStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ORD_Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_Status", x => x.Id);
                });

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
                name: "SYS_Function",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Roles", x => x.Id);
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
                name: "UPL_StockGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockGroupTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_StockGroup", x => x.Id);
                });

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
                name: "SYS_RolesToFunction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    FunctionID = table.Column<int>(type: "int", nullable: false),
                    CanAccess = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_RolesToFunction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_RolesToFunction_SYS_Function_FunctionID",
                        column: x => x.FunctionID,
                        principalTable: "SYS_Function",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SYS_RolesToFunction_SYS_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "SYS_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_SYS_Users_SYS_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "SYS_Roles",
                        principalColumn: "Id");
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
                    StakeHolderEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "UPL_Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    LocationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    Width = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    Depth = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UPL_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UPL_Location_UPL_UOM_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UPL_UOM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UPL_PartInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartCost = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
                    Width = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
                    Depth = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: true),
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
                    table.ForeignKey(
                        name: "FK_UPL_PartInfo_UPL_UOM_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UPL_UOM",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORD_Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_ORD_Order_ORD_Status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ORD_Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORD_Order_UPL_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "UPL_Project",
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
                name: "ORD_Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    POHeaderID = table.Column<int>(type: "int", nullable: true),
                    StockGroupID = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_ORD_Line_PO_Header_POHeaderID",
                        column: x => x.POHeaderID,
                        principalTable: "PO_Header",
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

            migrationBuilder.CreateTable(
                name: "PO_Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POHeaderID = table.Column<int>(type: "int", nullable: false),
                    PartID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsSerialized = table.Column<bool>(type: "bit", nullable: false),
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
                        name: "FK_PO_Line_UPL_PartInfo_PartID",
                        column: x => x.PartID,
                        principalTable: "UPL_PartInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptID = table.Column<int>(type: "int", nullable: false),
                    POLineID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_INV_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INV_Stock_INV_Receipt_ReceiptID",
                        column: x => x.ReceiptID,
                        principalTable: "INV_Receipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_INV_Stock_INV_StockStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "INV_StockStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ORD_PickList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineID = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORD_PickList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ORD_PickList_INV_Stock_StockID",
                        column: x => x.StockID,
                        principalTable: "INV_Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORD_PickList_ORD_Line_LineID",
                        column: x => x.LineID,
                        principalTable: "ORD_Line",
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
                name: "IX_INV_Stock_StatusID",
                table: "INV_Stock",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_Stock_StockGroupID",
                table: "INV_Stock",
                column: "StockGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_OrderID",
                table: "ORD_Line",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_PartID",
                table: "ORD_Line",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_POHeaderID",
                table: "ORD_Line",
                column: "POHeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Line_StockGroupID",
                table: "ORD_Line",
                column: "StockGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Order_ProjectID",
                table: "ORD_Order",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_Order_StatusID",
                table: "ORD_Order",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_PickList_LineID",
                table: "ORD_PickList",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_ORD_PickList_StockID",
                table: "ORD_PickList",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Header_ProjectID",
                table: "PO_Header",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Line_PartID",
                table: "PO_Line",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Line_POHeaderID",
                table: "PO_Line",
                column: "POHeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_ChangeLogDetails_ChangeLogId",
                table: "SYS_ChangeLogDetails",
                column: "ChangeLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_RolesToFunction_FunctionID",
                table: "SYS_RolesToFunction",
                column: "FunctionID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_RolesToFunction_RoleID",
                table: "SYS_RolesToFunction",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_Location_UOMID",
                table: "UPL_Location",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_PartInfo_UOMID",
                table: "UPL_PartInfo",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_UPL_Project_CustomerID",
                table: "UPL_Project",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORD_PickList");

            migrationBuilder.DropTable(
                name: "SYS_ChangeLogDetails");

            migrationBuilder.DropTable(
                name: "SYS_RolesToFunction");

            migrationBuilder.DropTable(
                name: "SYS_Users");

            migrationBuilder.DropTable(
                name: "INV_Stock");

            migrationBuilder.DropTable(
                name: "ORD_Line");

            migrationBuilder.DropTable(
                name: "SYS_ChangeLog");

            migrationBuilder.DropTable(
                name: "SYS_Function");

            migrationBuilder.DropTable(
                name: "SYS_Roles");

            migrationBuilder.DropTable(
                name: "INV_Receipt");

            migrationBuilder.DropTable(
                name: "INV_StockStatus");

            migrationBuilder.DropTable(
                name: "PO_Line");

            migrationBuilder.DropTable(
                name: "UPL_Location");

            migrationBuilder.DropTable(
                name: "ORD_Order");

            migrationBuilder.DropTable(
                name: "UPL_StockGroup");

            migrationBuilder.DropTable(
                name: "PO_Header");

            migrationBuilder.DropTable(
                name: "UPL_PartInfo");

            migrationBuilder.DropTable(
                name: "ORD_Status");

            migrationBuilder.DropTable(
                name: "UPL_Project");

            migrationBuilder.DropTable(
                name: "UPL_UOM");

            migrationBuilder.DropTable(
                name: "UPL_Customer");
        }
    }
}
