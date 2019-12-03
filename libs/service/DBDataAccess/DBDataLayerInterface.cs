using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TutorBits.DBDataAccess
{
    public interface DBDataLayerInterface
    {
        Task<T> Create<T>(T entity) where T : class, new();

        Task<T> Get<T>(params object[] keys) where T : class, new();

        Task<T> Get<T>(ICollection<object> keys) where T : class, new();

        Task<T> Get<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, params object[] keys) where T : class, new();

        Task<T> Get<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, ICollection<object> keys) where T : class, new();

        Task<ICollection<T>> GetAll<T>(Expression<Func<T, Boolean>> where = null, int? skip = null, int? take = null) where T : class, new();

        Task<ICollection<T>> GetAll<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, Expression<Func<T, Boolean>> where = null, int? skip = null, int? take = null) where T : class, new();

        Task Update<T>(T entity) where T : class, new();

        Task Delete<T>(T entity) where T : class, new();

        Task Delete<T>(params object[] keys) where T : class, new();

        Task Delete<T>(ICollection<object> keys) where T : class, new();
    }
}
