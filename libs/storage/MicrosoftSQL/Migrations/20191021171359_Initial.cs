using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tutorials",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutorials", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tutorials");
        }
    }
}
