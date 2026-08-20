using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Header1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Header2 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Header3 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Header4 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PrinterName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TicketsPerPage = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carriers",
                columns: table => new
                {
                    CarrierName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriers", x => x.CarrierName);
                });

            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    CommodityName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.CommodityName);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerName);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    DestinationName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.DestinationName);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationName);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Ticket = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Void = table.Column<bool>(type: "INTEGER", nullable: false),
                    InWeight = table.Column<int>(type: "INTEGER", nullable: false),
                    DateIn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOut = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OutWeight = table.Column<int>(type: "INTEGER", nullable: true),
                    Customer = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Commodity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Carrier = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    TruckId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Ticket);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Lot = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckId);
                });

            migrationBuilder.InsertData(
                table: "AppSetup",
                columns: new[] { "Id", "Header1", "Header2", "Header3", "Header4", "PrinterName", "TicketNumber", "TicketsPerPage" },
                values: new object[] { 1, "Basic Weigh", "", "", "", null, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Carrier",
                table: "Transactions",
                column: "Carrier");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Customer",
                table: "Transactions",
                column: "Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DateIn",
                table: "Transactions",
                column: "DateIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetup");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropTable(
                name: "Commodities");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Trucks");
        }
    }
}
