using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class TruckCarrierCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Trucks",
                table: "Trucks");

            migrationBuilder.AddColumn<string>(
                name: "CarrierName",
                table: "Trucks",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trucks",
                table: "Trucks",
                columns: new[] { "TruckId", "CarrierName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Trucks",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "CarrierName",
                table: "Trucks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trucks",
                table: "Trucks",
                column: "TruckId");
        }
    }
}
