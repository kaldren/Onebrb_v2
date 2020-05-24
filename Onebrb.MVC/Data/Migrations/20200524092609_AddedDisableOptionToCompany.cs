using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class AddedDisableOptionToCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Companies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Companies");
        }
    }
}
