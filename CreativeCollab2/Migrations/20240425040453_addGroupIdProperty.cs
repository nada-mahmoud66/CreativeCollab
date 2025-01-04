using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreativeCollab2.Migrations
{
    /// <inheritdoc />
    public partial class addGroupIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Notifications");
        }
    }
}
