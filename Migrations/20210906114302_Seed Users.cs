using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForecastAPI.Migrations
{
    public partial class SeedUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "admin@brainence.com", "Admin", "$2a$11$C4ET8f6NfM/t7eJJ8hG8deSGsx/4y3B8ut7Onv4ID11hWOUziUQy6", 0 },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), "admin2@brainence.com", "Admin2", "$2a$11$AHeCsg0BcSkFbG8Nge4aA.c8mSmSncftTmpCGK7kN4QtUg7aq6lc.", 0 },
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), "mark@brainence.com", "Mark", "$2a$11$ubNfpuPaWrXYTSbG7H05n.e70WfFE9kYCxF6KbZzcJAg4DlfMxmb2", 1 },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "iryna@brainence.com", "Iryna", "$2a$11$YRTE221fcY/XGTI5LnEaKu/a6cHHdyew0PlH/6OjN8f1x/DOzofTG", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
