using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    public partial class QuestionExampleCommentsRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMS",
                table: "Tutorials");

            migrationBuilder.RenameColumn(
                name: "TutorialType",
                table: "Tutorials",
                newName: "ProgrammingTopic");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Tutorials",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThumbnailId",
                table: "Tutorials",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                table: "Tutorials",
                nullable: true);

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
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    DurationMS = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    ProgrammingTopic = table.Column<string>(maxLength: 64, nullable: false),
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
                name: "Thumbnails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thumbnails_Accounts_OwnerAccountId",
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
                    TargetId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    Body = table.Column<string>(maxLength: 1028, nullable: true)
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
                        onDelete: ReferentialAction.Cascade);
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
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
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
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DateModified = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(maxLength: 1028, nullable: true),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Owner = table.Column<string>(maxLength: 1028, nullable: true),
                    OwnerAccountId = table.Column<Guid>(nullable: true),
                    DurationMS = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Examples",
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
                    ProgrammingTopic = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1028, nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true),
                    ThumbnailId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examples_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Examples_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Examples_Thumbnails_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Thumbnails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    TargetId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false)
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
                name: "ExampleComments",
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
                    table.PrimaryKey("PK_ExampleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleComments_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExampleComments_Examples_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Examples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExampleRatings",
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
                    table.PrimaryKey("PK_ExampleRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExampleRatings_Examples_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Examples",
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

            migrationBuilder.CreateTable(
                name: "ExampleCommentRatings",
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
                    table.PrimaryKey("PK_ExampleCommentRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExampleCommentRatings_Accounts_OwnerAccountId",
                        column: x => x.OwnerAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExampleCommentRatings_ExampleComments_TargetId",
                        column: x => x.TargetId,
                        principalTable: "ExampleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_ProjectId",
                table: "Tutorials",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_ThumbnailId",
                table: "Tutorials",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_Title",
                table: "Tutorials",
                column: "Title")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Tutorials_VideoId",
                table: "Tutorials",
                column: "VideoId");

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
                name: "IX_ExampleCommentRatings_OwnerAccountId",
                table: "ExampleCommentRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleCommentRatings_TargetId",
                table: "ExampleCommentRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleComments_OwnerAccountId",
                table: "ExampleComments",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleComments_TargetId",
                table: "ExampleComments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRatings_OwnerAccountId",
                table: "ExampleRatings",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExampleRatings_TargetId",
                table: "ExampleRatings",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_OwnerAccountId",
                table: "Examples",
                column: "OwnerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_ProjectId",
                table: "Examples",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_ThumbnailId",
                table: "Examples",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_Examples_Title",
                table: "Examples",
                column: "Title")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerAccountId",
                table: "Projects",
                column: "OwnerAccountId");

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
                name: "IX_Thumbnails_OwnerAccountId",
                table: "Thumbnails",
                column: "OwnerAccountId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Videos_OwnerAccountId",
                table: "Videos",
                column: "OwnerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Projects_ProjectId",
                table: "Tutorials",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Thumbnails_ThumbnailId",
                table: "Tutorials",
                column: "ThumbnailId",
                principalTable: "Thumbnails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutorials_Videos_VideoId",
                table: "Tutorials",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Projects_ProjectId",
                table: "Tutorials");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Thumbnails_ThumbnailId",
                table: "Tutorials");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutorials_Videos_VideoId",
                table: "Tutorials");

            migrationBuilder.DropTable(
                name: "AnswerCommentRatings");

            migrationBuilder.DropTable(
                name: "AnswerRatings");

            migrationBuilder.DropTable(
                name: "ExampleCommentRatings");

            migrationBuilder.DropTable(
                name: "ExampleRatings");

            migrationBuilder.DropTable(
                name: "QuestionCommentRatings");

            migrationBuilder.DropTable(
                name: "QuestionRatings");

            migrationBuilder.DropTable(
                name: "TutorialCommentRatings");

            migrationBuilder.DropTable(
                name: "TutorialRatings");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "AnswerComments");

            migrationBuilder.DropTable(
                name: "ExampleComments");

            migrationBuilder.DropTable(
                name: "QuestionComments");

            migrationBuilder.DropTable(
                name: "TutorialComments");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Thumbnails");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_ProjectId",
                table: "Tutorials");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_ThumbnailId",
                table: "Tutorials");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_Title",
                table: "Tutorials");

            migrationBuilder.DropIndex(
                name: "IX_Tutorials_VideoId",
                table: "Tutorials");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Email",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_NickName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "ThumbnailId",
                table: "Tutorials");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Tutorials");

            migrationBuilder.AddColumn<decimal>(
                name: "DurationMS",
                table: "Tutorials",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.RenameColumn(
                name: "ProgrammingTopic",
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
