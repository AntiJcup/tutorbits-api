using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TutorBits
{
    namespace Storage
    {
        namespace MicrosoftSQL
        {
            public class DesignTimeContextFactory : IDesignTimeDbContextFactory<TutorBitsSQLDbContext>
            {
                public TutorBitsSQLDbContext CreateDbContext(string[] args)
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                    var builder = new DbContextOptionsBuilder<TutorBitsSQLDbContext>();
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    builder.UseSqlServer(connectionString);
                    return new TutorBitsSQLDbContext(builder.Options);
                }
            }
        }
    }
}