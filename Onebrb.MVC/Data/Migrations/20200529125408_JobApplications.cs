using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class JobApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllApplications_AspNetUsers_ApplicationUserId",
                table: "AllApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_AllApplications_Jobs_JobId",
                table: "AllApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllApplications",
                table: "AllApplications");

            migrationBuilder.RenameTable(
                name: "AllApplications",
                newName: "JobApplications");

            migrationBuilder.RenameIndex(
                name: "IX_AllApplications_ApplicationUserId",
                table: "JobApplications",
                newName: "IX_JobApplications_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                columns: new[] { "JobId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicationUserId",
                table: "JobApplications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicationUserId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Jobs_JobId",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.RenameTable(
                name: "JobApplications",
                newName: "AllApplications");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_ApplicationUserId",
                table: "AllApplications",
                newName: "IX_AllApplications_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllApplications",
                table: "AllApplications",
                columns: new[] { "JobId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AllApplications_AspNetUsers_ApplicationUserId",
                table: "AllApplications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AllApplications_Jobs_JobId",
                table: "AllApplications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
