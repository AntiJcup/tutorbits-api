using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Email = table.Column<string>(maxLength: 1028, nullable: false),
                    NickName = table.Column<string>(maxLength: 256, nullable: false),
                    AcceptOffers = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tutorials",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 64, nullable: true),
                    TutorialType = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1028, nullable: true),
                    DurationMS = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutorials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tutorials_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OwnerAccountId",
                table: "Accounts",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_OwnerAccountId",
                table: "Tutorials",
                column: "OwnerAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tutorials");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
