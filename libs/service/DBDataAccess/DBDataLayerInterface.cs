using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TutorBits
{
    namespace DBDataAccess
    {
        public interface DBDataLayerInterface
        {
            Task<T> Create<T>(T entity) where T : class;

            Task<T> Get<T>(params object[] keys) where T : class;

            Task<T> Get<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, params object[] keys) where T : class;

            Task<ICollection<T>> GetAll<T>(Expression<Func<T, Boolean>> where, int? skip, int? take) where T : class;

            Task<ICollection<T>> GetAll<T, TProperty>(ICollection<Expression<Func<T, TProperty>>> includes, Expression<Func<T, Boolean>> where, int? skip, int? take) where T : class;

            Task Update<T>(T entity) where T : class;

            Task Delete<T>(T entity) where T : class;
        }
    }
}