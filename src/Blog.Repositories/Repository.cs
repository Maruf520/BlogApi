using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public class Repository //: IRepository
    {
       // private readonly ApplicationDbContext _dbContext;
        public Repository()
        {
            //_dbContext = context;
        }
//        public virtual IQueryable<TEntity> Query<TEntity>(
//    Expression<Func<TEntity, bool>> filter = null,
//    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//    Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null,
//    bool enableChangeTracking = false
//) where TEntity : BaseEntity
//        {
//            // Get the DbSet for the specified entity type
//            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

//            // Disable change tracking if specified
//            if (!enableChangeTracking)
//                query = query.AsNoTracking();

//            // Apply eager loading for related entities
//            if (includes != null)
//                query = includes(query);

//            // Apply filter if provided
//            if (filter != null)
//                query = query.Where(filter);

//            // Apply ordering if provided
//            if (orderBy != null)
//                query = orderBy(query);

//            return query;
//        }
    }
}
