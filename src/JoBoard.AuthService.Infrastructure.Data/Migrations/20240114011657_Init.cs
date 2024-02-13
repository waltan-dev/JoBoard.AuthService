using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoBoard.AuthService.Infrastructure.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    NewEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    RegisterConfirmToken = table.Column<string>(type: "text", nullable: true),
                    RegisterConfirmTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetPasswordConfirmToken = table.Column<string>(type: "text", nullable: true),
                    ResetPasswordConfirmTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangeEmailConfirmToken = table.Column<string>(type: "text", nullable: true),
                    ChangeEmailConfirmTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccountDeactivationConfirmToken = table.Column<string>(type: "text", nullable: true),
                    AccountDeactivationConfirmTokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalUserId = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAccounts", x => new { x.Id, x.ExternalUserId, x.Provider });
                    table.ForeignKey(
                        name: "FK_ExternalAccounts_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalAccounts_ExternalUserId_Provider",
                table: "ExternalAccounts",
                columns: new[] { "ExternalUserId", "Provider" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAccounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
