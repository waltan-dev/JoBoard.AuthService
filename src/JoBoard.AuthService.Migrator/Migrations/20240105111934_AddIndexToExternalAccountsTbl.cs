using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoBoard.AuthService.Migrator.Migrations
{
    public partial class AddIndexToExternalAccountsTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExternalAccounts_ExternalUserId_Provider",
                table: "ExternalAccounts",
                columns: new[] { "ExternalUserId", "Provider" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExternalAccounts_ExternalUserId_Provider",
                table: "ExternalAccounts");
        }
    }
}
