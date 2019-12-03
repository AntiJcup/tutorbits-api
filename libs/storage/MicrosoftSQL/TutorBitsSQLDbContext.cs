using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutorBits.Models.Common;

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

                public DbSet<Tutorial> Tutorials { get; set; }

                public DbSet<Account> Accounts { get; set; }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
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
                    var tutorialTypeConverter = new EnumToStringConverter<TutorialType>();
                    foreach (var publicPropertieBaseType in publicPropertieBaseTypes)
                    {
                        if (publicPropertieBaseType.GetProperties().Any(p => p.Name == "TutorialType"))
                        {
                            modelBuilder.Entity(publicPropertieBaseType)
                                                    .Property("TutorialType")
                                                    .HasConversion(tutorialTypeConverter)
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