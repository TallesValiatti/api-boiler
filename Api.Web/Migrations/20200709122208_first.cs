using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Web.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { new Guid("0a725c02-6345-4d65-84b7-ad32b1a535d0"), "talles.dsv@gmail.com", "Talles Valiatti", "2bCEa/4u7SincXcyaPZgHyeWaRYDaeCAQUhNhCSy6Yg=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
