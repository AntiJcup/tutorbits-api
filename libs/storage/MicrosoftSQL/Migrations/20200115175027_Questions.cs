using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    public partial class Questions : Migration
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
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1028);

            migrationBuilder.AlterColumn<bool>(
                name: "AcceptOffers",
                table: "Accounts",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    TutorialLanguage = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 2056, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorialComment",
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
                    TargetId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorialComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialComment_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialComment_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorialRating",
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
                    table.PrimaryKey("PK_TutorialRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialRating_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
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
                    TargetId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answer_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionComment",
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
                    TargetId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionComment_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionComment_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionRating",
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
                    table.PrimaryKey("PK_QuestionRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRating_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TutorialCommentRating",
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
                    table.PrimaryKey("PK_TutorialCommentRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialCommentRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialCommentRating_TutorialComment_TargetId",
                        column: x => x.TargetId,
                        principalTable: "TutorialComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerComment",
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
                    TargetId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerComment_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerComment_Answer_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerRating",
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
                    table.PrimaryKey("PK_AnswerRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerRating_Answer_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCommentRating",
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
                    table.PrimaryKey("PK_QuestionCommentRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCommentRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionCommentRating_QuestionComment_TargetId",
                        column: x => x.TargetId,
                        principalTable: "QuestionComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerCommentRating",
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
                    table.PrimaryKey("PK_AnswerCommentRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerCommentRating_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerCommentRating_AnswerComment_TargetId",
                        column: x => x.TargetId,
                        principalTable: "AnswerComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_Title",
                table: "Tutorials",
                column: "Title")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_NickName",
                table: "Accounts",
                column: "NickName")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_OwnerAccountId",
                table: "Answer",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_TargetId",
                table: "Answer",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComment_OwnerAccountId",
                table: "AnswerComment",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComment_TargetId",
                table: "AnswerComment",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerCommentRating_OwnerAccountId",
                table: "AnswerCommentRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerCommentRating_TargetId",
                table: "AnswerCommentRating",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRating_OwnerAccountId",
                table: "AnswerRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRating_TargetId",
                table: "AnswerRating",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionComment_OwnerAccountId",
                table: "QuestionComment",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionComment_TargetId",
                table: "QuestionComment",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCommentRating_OwnerAccountId",
                table: "QuestionCommentRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCommentRating_TargetId",
                table: "QuestionCommentRating",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRating_OwnerAccountId",
                table: "QuestionRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRating_TargetId",
                table: "QuestionRating",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_OwnerAccountId",
                table: "Questions",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Title",
                table: "Questions",
                column: "Title")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TutorialComment_OwnerAccountId",
                table: "TutorialComment",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialComment_TargetId",
                table: "TutorialComment",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialCommentRating_OwnerAccountId",
                table: "TutorialCommentRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialCommentRating_TargetId",
                table: "TutorialCommentRating",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialRating_OwnerAccountId",
                table: "TutorialRating",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialRating_TargetId",
                table: "TutorialRating",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerCommentRating");

            migrationBuilder.DropTable(
                name: "AnswerRating");

            migrationBuilder.DropTable(
                name: "QuestionCommentRating");

            migrationBuilder.DropTable(
                name: "QuestionRating");

            migrationBuilder.DropTable(
                name: "TutorialCommentRating");

            migrationBuilder.DropTable(
                name: "TutorialRating");

            migrationBuilder.DropTable(
                name: "AnswerComment");

            migrationBuilder.DropTable(
                name: "QuestionComment");

            migrationBuilder.DropTable(
                name: "TutorialComment");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_Title",
                table: "Tutorials");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Email",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_NickName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TutorialCategory",
                table: "Tutorials");

            migrationBuilder.RenameColumn(
                name: "TutorialLanguage",
                table: "Tutorials",
                newName: "TutorialType");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Accounts",
                maxLength: 1028,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<bool>(
                name: "AcceptOffers",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
