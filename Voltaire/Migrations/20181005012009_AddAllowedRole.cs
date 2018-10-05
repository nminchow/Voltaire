using Microsoft.EntityFrameworkCore.Migrations;

namespace Voltaire.Migrations
{
    public partial class AddAllowedRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedRole",
                table: "Guilds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedRole",
                table: "Guilds");
        }
    }
}
