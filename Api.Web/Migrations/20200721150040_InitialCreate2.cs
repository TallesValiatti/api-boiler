using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Web.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthRefreshTokenValidation",
                table: "SignInRegister",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 7, 21, 14, 54, 48, 113, DateTimeKind.Utc).AddTicks(30));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthRefreshTokenValidation",
                table: "SignInRegister",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 21, 14, 54, 48, 113, DateTimeKind.Utc).AddTicks(30),
                oldClrType: typeof(DateTime));
        }
    }
}
