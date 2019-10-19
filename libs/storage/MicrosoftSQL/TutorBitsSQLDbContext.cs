using System;
using Microsoft.EntityFrameworkCore;
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
            }
        }
    }
}