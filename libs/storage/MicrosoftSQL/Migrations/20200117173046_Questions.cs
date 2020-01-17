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
                newName: "TutorialTopic");

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
                    QuestionTopic = table.Column<string>(maxLength: 64, nullable: false),
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
                name: "TutorialComments",
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
                    Body = table.Column<string>(maxLength: 1028, nullable: true),
                    TargetId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorialComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialComments_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialComments_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorialRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorialRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialRatings_Tutorials_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Body = table.Column<string>(maxLength: 1028, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Body = table.Column<string>(maxLength: 1028, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionComments_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionComments_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionRatings_Questions_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TutorialCommentRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    TargetId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorialCommentRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorialCommentRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorialCommentRatings_TutorialComments_TargetId",
                        column: x => x.TargetId,
                        principalTable: "TutorialComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Body = table.Column<string>(maxLength: 1028, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerComments_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerComments_Answers_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerRatings_Answers_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCommentRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCommentRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCommentRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionCommentRatings_QuestionComments_TargetId",
                        column: x => x.TargetId,
                        principalTable: "QuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerCommentRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerCommentRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerCommentRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnswerCommentRatings_AnswerComments_TargetId",
                        column: x => x.TargetId,
                        principalTable: "AnswerComments",
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
                name: "IX_AnswerCommentRatings_OwnerAccountId",
                table: "AnswerCommentRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerCommentRatings_TargetId",
                table: "AnswerCommentRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComments_OwnerAccountId",
                table: "AnswerComments",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerComments_TargetId",
                table: "AnswerComments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRatings_OwnerAccountId",
                table: "AnswerRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerRatings_TargetId",
                table: "AnswerRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OwnerAccountId",
                table: "Answers",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TargetId",
                table: "Answers",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCommentRatings_OwnerAccountId",
                table: "QuestionCommentRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCommentRatings_TargetId",
                table: "QuestionCommentRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionComments_OwnerAccountId",
                table: "QuestionComments",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionComments_TargetId",
                table: "QuestionComments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatings_OwnerAccountId",
                table: "QuestionRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionRatings_TargetId",
                table: "QuestionRatings",
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
                name: "IX_TutorialCommentRatings_OwnerAccountId",
                table: "TutorialCommentRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialCommentRatings_TargetId",
                table: "TutorialCommentRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialComments_OwnerAccountId",
                table: "TutorialComments",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialComments_TargetId",
                table: "TutorialComments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialRatings_OwnerAccountId",
                table: "TutorialRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorialRatings_TargetId",
                table: "TutorialRatings",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerCommentRatings");

            migrationBuilder.DropTable(
                name: "AnswerRatings");

            migrationBuilder.DropTable(
                name: "QuestionCommentRatings");

            migrationBuilder.DropTable(
                name: "QuestionRatings");

            migrationBuilder.DropTable(
                name: "TutorialCommentRatings");

            migrationBuilder.DropTable(
                name: "TutorialRatings");

            migrationBuilder.DropTable(
                name: "AnswerComments");

            migrationBuilder.DropTable(
                name: "QuestionComments");

            migrationBuilder.DropTable(
                name: "TutorialComments");

            migrationBuilder.DropTable(
                name: "Answers");

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
                name: "TutorialTopic",
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
