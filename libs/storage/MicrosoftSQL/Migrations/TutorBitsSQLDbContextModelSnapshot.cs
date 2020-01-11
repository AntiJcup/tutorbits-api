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

                    b.Property<bool>("AcceptOffers");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(1028);

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

                    b.HasIndex("OwnerAccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .HasMaxLength(1028);

                    b.Property<int>("CommentType");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Status");

                    b.Property<Guid>("TargetId");

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateModified")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<Guid?>("OwnerAccountId");

                    b.Property<int>("Score");

                    b.Property<int>("Status");

                    b.Property<Guid>("TargetId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.HasIndex("TargetId");

                    b.ToTable("Ratings");
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

                    b.Property<string>("TutorialLanguage")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("OwnerAccountId");

                    b.ToTable("Tutorials");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Account", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");
                });

            modelBuilder.Entity("TutorBits.Models.Common.Comment", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Tutorial", "Target")
                        .WithMany("Comments")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TutorBits.Models.Common.Rating", b =>
                {
                    b.HasOne("TutorBits.Models.Common.Account", "OwnerAccount")
                        .WithMany()
                        .HasForeignKey("OwnerAccountId");

                    b.HasOne("TutorBits.Models.Common.Tutorial", "Target")
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
#pragma warning restore 612, 618
        }
    }
}
