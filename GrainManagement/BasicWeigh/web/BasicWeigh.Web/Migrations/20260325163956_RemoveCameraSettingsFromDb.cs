using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCameraSettingsFromDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CameraBrand",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "CameraIp",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "CameraPassword",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "CameraTimeoutSeconds",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "CameraUrl",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "CameraUser",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "UsbCommand",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "UsbDeviceName",
                table: "AppSetup");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CameraBrand",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CameraIp",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CameraPassword",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CameraTimeoutSeconds",
                table: "AppSetup",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CameraUrl",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CameraUser",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsbCommand",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsbDeviceName",
                table: "AppSetup",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppSetup",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CameraBrand", "CameraIp", "CameraPassword", "CameraTimeoutSeconds", "CameraUrl", "CameraUser", "UsbCommand", "UsbDeviceName" },
                values: new object[] { "Generic IP Camera", "192.168.1.100", "password", 10, "", "admin", "", "" });
        }
    }
}
