using Blog.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public interface IRepository
    {
        IQueryable<TEntity> Query<TEntity>(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
        bool enableChangeTracking = false
    ) where TEntity : BaseEntity;
        Task AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}
