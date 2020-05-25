using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class stringAsPkForJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "Jobs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs",
                column: "Id");
        }
    }
}
