using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreativeCollab2.Migrations
{
    /// <inheritdoc />
    public partial class addNotificationMessageProprety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationMessage",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationMessage",
                table: "Notifications");
        }
    }
}
