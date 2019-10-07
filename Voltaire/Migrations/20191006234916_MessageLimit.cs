using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Voltaire.Migrations
{
    public partial class MessageLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessagesSentThisMonth",
                table: "Guilds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TrackingMonth",
                table: "Guilds",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessagesSentThisMonth",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "TrackingMonth",
                table: "Guilds");
        }
    }
}
