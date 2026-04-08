using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceTruckPhoneLotWithDescriptionNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lot",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Trucks");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trucks",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Trucks",
                type: "TEXT",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Trucks");

            migrationBuilder.AddColumn<string>(
                name: "Lot",
                table: "Trucks",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Trucks",
                type: "TEXT",
                maxLength: 20,
                nullable: true);
        }
    }
}
