using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class AddedApplicantsToJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Applicants",
                table: "Jobs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applicants",
                table: "Jobs");
        }
    }
}
