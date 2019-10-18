using System;
using Microsoft.EntityFrameworkCore;

namespace MicrosoftSQL
{
    public class SQLDbContext : DbContext
    {
        public SQLDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
