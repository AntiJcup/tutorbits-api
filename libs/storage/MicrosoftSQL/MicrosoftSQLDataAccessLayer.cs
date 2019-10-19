using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.DataAccess;

namespace TutorBits
{
    namespace Storage
    {
        namespace MicrosoftSQL
        {
            public static class ServiceExtensions
            {
                public static IServiceCollection AddMicrosoftSQLDataAccessLayer(this IServiceCollection services)
                {
                    services.AddTransient<DataLayerInterface, MicrosoftSQLDataAccessLayer>();
                    return services.AddTransient<DataAccessService>();
                }
            }

            public class MicrosoftSQLDataAccessLayer : DataLayerInterface
            {
                private readonly TutorBitsSQLDbContext dbContext_;

                public MicrosoftSQLDataAccessLayer(TutorBitsSQLDbContext dbContext)
                {
                    dbContext_ = dbContext;
                }

                public async Task Create<T>(T entity) where T : class
                {
                    await dbContext_.AddAsync(entity);
                }

                public async Task Delete<T>(T entity) where T : class
                {
                    dbContext_.Remove(entity);
                    await dbContext_.SaveChangesAsync();
                }

                public async Task<T> Get<T>(params object[] keys) where T : class
                {
                    return await dbContext_.FindAsync<T>(keys);
                }

                public async Task<T> Get<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, params object[] keys) where T : class
                {
                    var dbSet = dbContext_.Set<T>();

                    foreach (var include in includes)
                    {
                        dbSet.Include(include);
                    }

                    return await dbSet.FindAsync(keys);
                }

                public async Task<ICollection<T>> GetAll<T>(Expression<Func<T, bool>> where, int? skip = null, int? take = null) where T : class
                {
                    var dbSet = dbContext_.Set<T>();

                    if (where != null)
                    {
                        dbSet.Where(where);
                    }

                    if (skip.HasValue)
                    {
                        dbSet.Skip(skip.Value);
                    }

                    if (take.HasValue)
                    {
                        dbSet.Take(take.Value);
                    }

                    return await dbSet.ToListAsync();
                }

                public async Task<ICollection<T>> GetAll<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, Expression<Func<T, bool>> where, int? skip, int? take) where T : class
                {
                    var dbSet = dbContext_.Set<T>();

                    foreach (var include in includes)
                    {
                        dbSet.Include(include);
                    }

                    if (where != null)
                    {
                        dbSet.Where(where);
                    }

                    if (skip.HasValue)
                    {
                        dbSet.Skip(skip.Value);
                    }

                    if (take.HasValue)
                    {
                        dbSet.Take(take.Value);
                    }

                    return await dbSet.ToListAsync();
                }

                public async Task Update<T>(T entity) where T : class
                {
                    dbContext_.Update(entity);
                    await dbContext_.SaveChangesAsync();
                }
            }
        }
    }
}