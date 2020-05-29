using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class LogoFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
