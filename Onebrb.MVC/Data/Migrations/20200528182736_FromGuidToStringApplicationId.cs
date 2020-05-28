using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Onebrb.MVC.Data.Migrations
{
    public partial class FromGuidToStringApplicationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "AllApplications",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "AllApplications",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
