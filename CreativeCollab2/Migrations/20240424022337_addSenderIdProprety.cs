using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreativeCollab2.Migrations
{
    /// <inheritdoc />
    public partial class addSenderIdProprety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");
        }
    }
}
