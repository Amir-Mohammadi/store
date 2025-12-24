using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool saveNow = true, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);
        Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);
        Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetQuery();
        IQueryable<TResult> GetQuery<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken = default) where TProperty : class;
        Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken = default) where TProperty : class;
        bool IsModified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property);
        TProperty OriginalValue<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property);
    }
}
