using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUseAtKioskToMasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Trucks",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Destinations",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Commodities",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseAtKiosk",
                table: "Carriers",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "UseAtKiosk",
                table: "Carriers");
        }
    }
}
