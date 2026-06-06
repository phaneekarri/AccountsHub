using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthEntities.Migrations
{
    /// <inheritdoc />
    public partial class AddInternalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps");

            migrationBuilder.DropIndex(
                name: "IX_Otps_UserId",
                table: "Otps");

            migrationBuilder.DropColumn(
                name: "OtpCode",
                table: "Otps");

            migrationBuilder.RenameColumn(
                name: "ExpiresInSecs",
                table: "Otps",
                newName: "ExpirySpan");

            migrationBuilder.RenameColumn(
                name: "ExpiresInSecs",
                table: "AuthTokens",
                newName: "ExpirySpan");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "AuthTokens",
                newName: "Token");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpiryIn",
                table: "Otps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpiryIn",
                table: "AuthTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InternalUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMFAEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MFAEnabledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPassWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryIn = table.Column<int>(type: "int", nullable: false),
                    ExpirySpan = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPassWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPassWords_InternalUsers_InternalUserId",
                        column: x => x.InternalUserId,
                        principalTable: "InternalUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalUsers_UserId",
                table: "InternalUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassWords_InternalUserId",
                table: "UserPassWords",
                column: "InternalUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPassWords");

            migrationBuilder.DropTable(
                name: "InternalUsers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpiryIn",
                table: "Otps");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Otps");

            migrationBuilder.DropColumn(
                name: "ExpiryIn",
                table: "AuthTokens");

            migrationBuilder.RenameColumn(
                name: "ExpirySpan",
                table: "Otps",
                newName: "ExpiresInSecs");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AuthTokens",
                newName: "AccessToken");

            migrationBuilder.RenameColumn(
                name: "ExpirySpan",
                table: "AuthTokens",
                newName: "ExpiresInSecs");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtpCode",
                table: "Otps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Otps_UserId",
                table: "Otps",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
