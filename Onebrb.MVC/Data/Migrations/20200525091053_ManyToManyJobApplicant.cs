using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class ManyToManyJobApplicant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applicants",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "Applications",
                table: "Jobs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationUserJob",
                columns: table => new
                {
                    JobId = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserJob", x => new { x.JobId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserJob_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserJob_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserJob_ApplicationUserId",
                table: "ApplicationUserJob",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserJob");

            migrationBuilder.DropColumn(
                name: "Applications",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "Applicants",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
