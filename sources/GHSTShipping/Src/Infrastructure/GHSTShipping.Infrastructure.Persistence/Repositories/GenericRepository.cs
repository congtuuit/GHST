using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>(DbContext dbContext) : IGenericRepository<T> where T : class
    {
        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbContext
                 .Set<T>()
                 .AsNoTracking()
                 .ToListAsync();
        }

        public IQueryable<T> All()
        {
            return dbContext.Set<T>();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate);
        }

        public IQueryable<T> FromSqlRaw(string sql, params object[] parameters)
        {
            return dbContext.Set<T>().FromSqlRaw(sql, parameters);
        }

        public IQueryable<T> FromSql(FormattableString sql) => dbContext.Set<T>().FromSql(sql);

        public void ModifyRange(IEnumerable<T> entities)
        {
            dbContext.AttachRange(entities);
        }

        public void ModifyProperty<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression)
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).Property(propertyExpression).IsModified = true;
        }

        protected async Task<PaginationResponseDto<TEntity>> Paged<TEntity>(IQueryable<TEntity> query, int pageNumber, int pageSize) where TEntity : class
        {
            var count = await query.CountAsync();

            var pagedResult = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new(pagedResult, count, pageNumber, pageSize);
        }
    }
}
