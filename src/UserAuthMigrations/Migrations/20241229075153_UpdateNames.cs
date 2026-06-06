using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthEntities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Otps",
                table: "Otps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthTokens",
                table: "AuthTokens");

            migrationBuilder.RenameTable(
                name: "Otps",
                newName: "UserOtps");

            migrationBuilder.RenameTable(
                name: "AuthTokens",
                newName: "UserAccessTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOtps",
                table: "UserOtps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccessTokens",
                table: "UserAccessTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOtps",
                table: "UserOtps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccessTokens",
                table: "UserAccessTokens");

            migrationBuilder.RenameTable(
                name: "UserOtps",
                newName: "Otps");

            migrationBuilder.RenameTable(
                name: "UserAccessTokens",
                newName: "AuthTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Otps",
                table: "Otps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTokens",
                table: "AuthTokens",
                column: "Id");
        }
    }
}
