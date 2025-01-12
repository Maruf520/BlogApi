using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> Query<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
            bool enableChangeTracking = false
        ) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (!enableChangeTracking)
                query = query.AsNoTracking();

            if (includes != null)
                query = includes(query);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }
        public async Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsNoTracking();

            query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
