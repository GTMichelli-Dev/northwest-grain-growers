using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicWeigh.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCameraConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CameraId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CameraBrand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CameraIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CameraUser = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CameraPassword = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UsbDeviceName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CameraUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UsbCommand = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    TimeoutSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraConfigs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CameraConfigs_CameraId",
                table: "CameraConfigs",
                column: "CameraId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CameraConfigs");
        }
    }
}
