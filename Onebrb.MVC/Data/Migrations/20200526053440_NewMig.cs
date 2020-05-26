using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class NewMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserJob_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserJob");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserJob_Jobs_JobId",
                table: "ApplicationUserJob");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserJob",
                table: "ApplicationUserJob");

            migrationBuilder.RenameTable(
                name: "ApplicationUserJob",
                newName: "AllApplications");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserJob_ApplicationUserId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "ApplicationUserJob");

            migrationBuilder.RenameIndex(
                name: "IX_AllApplications_ApplicationUserId",
                table: "ApplicationUserJob",
                newName: "IX_ApplicationUserJob_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserJob",
                table: "ApplicationUserJob",
                columns: new[] { "JobId", "ApplicationUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserJob_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserJob",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserJob_Jobs_JobId",
                table: "ApplicationUserJob",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
