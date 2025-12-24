using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // Add
        public async Task AddAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        // Update
        public async Task UpdateAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            _dbSet.UpdateRange(entities);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        // Delete
        public async Task DeleteAsync(TEntity entity, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            var entities = _dbSet.Where(predicate);
            _dbSet.RemoveRange(entities);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, bool saveNow = true, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);
            if (saveNow)
                await _context.SaveChangesAsync(cancellationToken);
        }

        // Get
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).Select(selector).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                return await _dbSet.ToListAsync(cancellationToken);
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                return await _dbSet.Select(selector).ToListAsync(cancellationToken);
            return await _dbSet.Where(predicate).Select(selector).ToListAsync(cancellationToken);
        }

        // Query
        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TResult> GetQuery<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return _dbSet.Select(selector).AsQueryable();
        }

        // Load related data
        public async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken = default) where TProperty : class
        {
            await _context.Entry(entity).Collection(collectionProperty).LoadAsync(cancellationToken);
        }

        public async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken = default) where TProperty : class
        {
            await _context.Entry(entity).Reference(referenceProperty).LoadAsync(cancellationToken);
        }

        // Tracking
        public bool IsModified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
        {
            var entry = _context.Entry(entity);
            var propName = ((MemberExpression)property.Body).Member.Name;
            return entry.Property(propName).IsModified;
        }

        public TProperty OriginalValue<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
        {
            var entry = _context.Entry(entity);
            var propName = ((MemberExpression)property.Body).Member.Name;
            return (TProperty)entry.Property(propName).OriginalValue;
        }
    }
}
