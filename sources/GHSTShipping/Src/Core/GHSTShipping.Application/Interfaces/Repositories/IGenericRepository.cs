using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        void Modify(T entities);
        void ModifyRange(IEnumerable<T> entities);
        void ModifyProperty<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression);

        IQueryable<T> All();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> FromSqlRaw(string sql, params object[] parameters);
        IQueryable<T> FromSql(FormattableString sql);
    }
}
