using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPrinterAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InboundPrinterId",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KioskPrinterId",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutboundPrinterId",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppSetup",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "InboundPrinterId", "KioskPrinterId", "OutboundPrinterId" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InboundPrinterId",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "KioskPrinterId",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "OutboundPrinterId",
                table: "AppSetup");
        }
    }
}
