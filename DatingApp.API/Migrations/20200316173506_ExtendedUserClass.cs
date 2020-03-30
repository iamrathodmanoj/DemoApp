using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class ExtendedUserClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "USers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "USers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KnownAs",
                table: "USers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "USers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LookingFor",
                table: "USers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_USers_UserId",
                        column: x => x.UserId,
                        principalTable: "USers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropColumn(
                name: "City",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "KnownAs",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "USers");

            migrationBuilder.DropColumn(
                name: "LookingFor",
                table: "USers");
        }
    }
}
