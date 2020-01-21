using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutorBits.Models.Common;
using Toolbelt.ComponentModel.DataAnnotations;

namespace TutorBits
{
    namespace Storage
    {
        namespace MicrosoftSQL
        {
            public class TutorBitsSQLDbContext : DbContext
            {
                public TutorBitsSQLDbContext(DbContextOptions options) : base(options)
                {

                }

                //READ THIS PLEASE
                //ADD ALL MODELS HERE. MIGRATION USES IT TO AUTO CONVERT CERTAIN PROPERTIES

                public DbSet<Tutorial> Tutorials { get; set; }
                public DbSet<TutorialRating> TutorialRatings { get; set; }
                public DbSet<TutorialComment> TutorialComments { get; set; }
                public DbSet<TutorialCommentRating> TutorialCommentRatings { get; set; }

                public DbSet<Example> Examples { get; set; }
                public DbSet<ExampleRating> ExampleRatings { get; set; }
                public DbSet<ExampleComment> ExampleComments { get; set; }
                public DbSet<ExampleCommentRating> ExampleCommentRatings { get; set; }

                public DbSet<Account> Accounts { get; set; }

                public DbSet<Question> Questions { get; set; }
                public DbSet<QuestionRating> QuestionRatings { get; set; }
                public DbSet<QuestionComment> QuestionComments { get; set; }
                public DbSet<QuestionCommentRating> QuestionCommentRatings { get; set; }


                public DbSet<Answer> Answers { get; set; }
                public DbSet<AnswerRating> AnswerRatings { get; set; }
                public DbSet<AnswerComment> AnswerComments { get; set; }
                public DbSet<AnswerCommentRating> AnswerCommentRatings { get; set; }

                public DbSet<Video> Videos { get; set; }
                public DbSet<Thumbnail> Thumbnails { get; set; }
                public DbSet<Project> Projects { get; set; }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    base.OnModelCreating(modelBuilder);

                    modelBuilder.BuildIndexesFromAnnotationsForSqlServer();

                    //Auto generates tables that are of base type
                    var publicPropertieBaseTypes = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p => p.PropertyType.IsGenericType &&
                                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                            p.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(BaseModel)))
                                .Select(p => p.PropertyType.GetGenericArguments()[0]);

                    //Setup default values for base class while maintaining individual tables
                    //modelBuilder.Entity(typeof(Base)).Property("Status").HasConversion(converter); 
                    //Creates the base table ignoring other tables including attr and totable
                    var baseStateConverter = new EnumToStringConverter<BaseState>();
                    var programmingTopicConverter = new EnumToStringConverter<ProgrammingTopic>();
                    var commentTypeConverter = new EnumToStringConverter<CommentType>();
                    foreach (var publicPropertieBaseType in publicPropertieBaseTypes)
                    {
                        if (publicPropertieBaseType.GetProperties().Any(p => p.Name == "ProgrammingTopic"))
                        {
                            modelBuilder.Entity(publicPropertieBaseType)
                                                    .Property("ProgrammingTopic")
                                                    .HasConversion(programmingTopicConverter)
                                                    .HasMaxLength(64);
                        }

                        if (publicPropertieBaseType.GetProperties().Any(p => p.Name == "CommentType"))
                        {
                            modelBuilder.Entity(publicPropertieBaseType)
                                                    .Property("CommentType")
                                                    .HasConversion(commentTypeConverter)
                                                    .HasMaxLength(64);
                        }

                        modelBuilder.Entity(publicPropertieBaseType)
                                                .Property("Status")
                                                .HasConversion(baseStateConverter)
                                                .HasMaxLength(64);

                        modelBuilder.Entity(publicPropertieBaseType, c =>
                        {
                            c.Property("DateCreated").HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
                            c.Property("DateModified").HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAddOrUpdate();
                        });
                    }
                }
            }
        }
    }
}