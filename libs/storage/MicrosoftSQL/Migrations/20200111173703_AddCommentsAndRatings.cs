using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    public partial class AddCommentsAndRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TutorialType",
                table: "Tutorials",
                newName: "TutorialLanguage");

            migrationBuilder.AddColumn<string>(
                name: "TutorialCategory",
                table: "Tutorials",
                maxLength: 64,
                nullable: false,
                defaultValue: "Tutorial");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Body = table.Column<string>(maxLength: 1028, nullable: true),
                    CommentType = table.Column<int>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerAccountId",
                table: "Comments",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TargetId",
                table: "Comments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OwnerAccountId",
                table: "Ratings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TargetId",
                table: "Ratings",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropColumn(
                name: "TutorialCategory",
                table: "Tutorials");

            migrationBuilder.RenameColumn(
                name: "TutorialLanguage",
                table: "Tutorials",
                newName: "TutorialType");
        }
    }
}
