using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoBoard.AuthService.Infrastructure.Data.Migrations
{
    public partial class UpdateTokensFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewEmailConfirmationTokenExpiration",
                table: "Users",
                newName: "ChangeEmailConfirmTokenExpiration");

            migrationBuilder.RenameColumn(
                name: "NewEmailConfirmationToken",
                table: "Users",
                newName: "ChangeEmailConfirmToken");

            migrationBuilder.AddColumn<string>(
                name: "AccountDeactivationConfirmToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AccountDeactivationConfirmTokenExpiration",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountDeactivationConfirmToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountDeactivationConfirmTokenExpiration",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChangeEmailConfirmTokenExpiration",
                table: "Users",
                newName: "NewEmailConfirmationTokenExpiration");

            migrationBuilder.RenameColumn(
                name: "ChangeEmailConfirmToken",
                table: "Users",
                newName: "NewEmailConfirmationToken");
        }
    }
}
