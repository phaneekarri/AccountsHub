using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthMigrations.Migrations
{
    /// <inheritdoc />
    public partial class RedesignAuthEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryIn = table.Column<int>(type: "int", nullable: false),
                    ExpirySpan = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOtps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserIdentifierType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    OtpType = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryIn = table.Column<int>(type: "int", nullable: false),
                    ExpirySpan = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOtps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MfaEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MfaMethod = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MethodType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Provider = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProviderUserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProviderEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VerificationType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPasswordChange = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordExpiry = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "DATEADD(day, 90, GETUTCDATE())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthMethods_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethods_Identifier",
                table: "AuthMethods",
                column: "Identifier",
                unique: true,
                filter: "[Identifier] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethods_Provider_ProviderUserId",
                table: "AuthMethods",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true,
                filter: "[Provider] IS NOT NULL AND [ProviderUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethods_UserId",
                table: "AuthMethods",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethods_UserId_VerificationType",
                table: "AuthMethods",
                columns: new[] { "UserId", "VerificationType" },
                unique: true,
                filter: "[VerificationType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethods_UserName",
                table: "AuthMethods",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthMethods");

            migrationBuilder.DropTable(
                name: "UserAccessTokens");

            migrationBuilder.DropTable(
                name: "UserOtps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
