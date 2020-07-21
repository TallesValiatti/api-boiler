using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SignInRegister",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE ()"),
                    UserId = table.Column<Guid>(nullable: false),
                    AuthToken = table.Column<string>(nullable: false),
                    AuthRefreshToken = table.Column<string>(nullable: false),
                    SignInType = table.Column<int>(nullable: false),
                    AuthRefreshTokenValidation = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 21, 14, 54, 48, 113, DateTimeKind.Utc).AddTicks(30))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignInRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignInRegister_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "PasswordHash" },
                values: new object[] { new Guid("0a725c02-6345-4d65-84b7-ad32b1a535d0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "talles.dsv@gmail.com", "Talles Valiatti", "2bCEa/4u7SincXcyaPZgHyeWaRYDaeCAQUhNhCSy6Yg=" });

            migrationBuilder.CreateIndex(
                name: "IX_SignInRegister_UserId",
                table: "SignInRegister",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignInRegister");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
