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

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Accounts");
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

                    b.Property<string>("Language")
                        .HasMaxLength(64);

                    b.Property<string>("Notes")
                        .HasMaxLength(1028);

                    b.Property<string>("Owner")
                        .HasMaxLength(1028);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Title")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Tutorials");
                });
#pragma warning restore 612, 618
        }
    }
}
