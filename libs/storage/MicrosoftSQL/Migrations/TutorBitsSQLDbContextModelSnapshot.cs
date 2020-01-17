﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutorBits.Storage.MicrosoftSQL;

namespace TutorBits.Storage.MicrosoftSQL.Migrations
{
    [DbContext(typeof(TutorBitsSQLDbContext))]
    partial class TutorBitsSQLDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TutorBits.Models.Common.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool?>("AcceptOffers");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("NickName")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("OwnerAccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(1028);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(1028);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("AnswerComments");
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerCommentRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("AnswerCommentRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("AnswerRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(2056);

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("QuestionTopic")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("Title")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(1028);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("QuestionComments");
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionCommentRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("QuestionCommentRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("QuestionRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Tutorial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(1028);

                    b.Property<decimal>("DurationMS")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Title")
                        .HasMaxLength(64);

                    b.Property<string>("TutorialCategory")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("TutorialTopic")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("Title")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.ToTable("Tutorials");
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(1028);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid?>("TargetId");

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("TutorialComments");
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialCommentRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("TutorialCommentRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("TutorialRatings");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Account", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Answer", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Question", "Target")
                        .WithMany("Answers")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerComment", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Answer", "Target")
                        .WithMany("Comments")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerCommentRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.AnswerComment", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.AnswerRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Answer", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.Question", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionComment", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Question", "Target")
                        .WithMany("Comments")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionCommentRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.QuestionComment", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.QuestionRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Question", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.Tutorial", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialComment", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Tutorial", "Target")
                        .WithMany("Comments")
                        .HasForeignKey("TargetId");
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialCommentRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.TutorialComment", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.TutorialRating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Tutorial", "Target")
                        .WithMany("Ratings")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
