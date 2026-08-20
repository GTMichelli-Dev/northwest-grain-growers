using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddKioskPromptsToSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskCarrier",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskCommodity",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskCustomer",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskDestinationOnInbound",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskDestinationOnOutbound",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "PromptKioskLocation",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AppSetup",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PromptKioskCarrier", "PromptKioskCommodity", "PromptKioskCustomer", "PromptKioskDestinationOnInbound", "PromptKioskDestinationOnOutbound", "PromptKioskLocation" },
                values: new object[] { true, true, true, false, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromptKioskCarrier",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "PromptKioskCommodity",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "PromptKioskCustomer",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "PromptKioskDestinationOnInbound",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "PromptKioskDestinationOnOutbound",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "PromptKioskLocation",
                table: "AppSetup");
        }
    }
}
