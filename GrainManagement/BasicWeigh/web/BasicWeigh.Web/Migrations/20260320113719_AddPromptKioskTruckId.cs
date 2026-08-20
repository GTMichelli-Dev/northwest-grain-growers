using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPromptKioskTruckId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskTruckId",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AppSetup",
                keyColumn: "Id",
                keyValue: 1,
                column: "PromptKioskTruckId",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromptKioskTruckId",
                table: "AppSetup");
        }
    }
}
