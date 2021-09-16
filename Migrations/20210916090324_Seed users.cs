using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForecastAPI.Migrations
{
    public partial class Seedusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "admin@brainence.com", "Admin", "$2a$11$ghVeDxJnFkYs8bTTgM4WdeEZ76NkRdIHNp/QY2o8hy8Rcvx/EjwKm", 0 },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), "admin2@brainence.com", "Admin2", "$2a$11$E5gjPw6XuNnob1511witn.u4/sG75QgqGmA8/n2pibNIcyqsnBJki", 0 },
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), "mark@brainence.com", "Mark", "$2a$11$STG3hRKruvyP.3SqcUrhSOjGIzg.HlJdBMZger308IGhwG3q252T6", 1 },
                    { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "iryna@brainence.com", "Iryna", "$2a$11$BKItgl0UcRQLQmZsorJaC.SaPnYY3jtN86u7Mhm/74aynZa8lHtp6", 1 }
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
