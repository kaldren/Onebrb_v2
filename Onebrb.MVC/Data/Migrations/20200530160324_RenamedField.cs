using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class RenamedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
