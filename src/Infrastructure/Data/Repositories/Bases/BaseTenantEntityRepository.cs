using Core.Entities.Bases;
using Core.Exceptions;
using Core.Interfaces.Repositories.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories.Bases
{
    public abstract class BaseTenantEntityRepository<TEntity> : IBaseTenantEntityRepository<TEntity> where TEntity : BaseTenantEntity
    {
        private readonly DefaultContext _context;

        public DbSet<TEntity> DBSet { get; private set; }
        public Guid TenantId { get; private set; }
        public IBaseTenantEntityRepository<TEntity> SetTenantId(Guid tenantId)
        {
            TenantId = tenantId;
            return this;
        }
        protected BaseTenantEntityRepository(DefaultContext context)
        {
            _context = context;
            DBSet = context.Set<TEntity>();
        }
        public IQueryable<TEntity> GetDbSet(bool ignoreRemovedItens = true) => ignoreRemovedItens ? AllNotRemoved() : DBSet;
        public IQueryable<TEntity> GetTenantDbSet(bool ignoreRemovedItens = true) => ignoreRemovedItens ? TenantAllNotRemoved().Where(e => e.TenantId == TenantId) : DBSet.Where(e => e.TenantId == TenantId);
        public async virtual Task<IEnumerable<TEntity>> GetMany(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.Where(where);

            if (include != null)
                dbSet = include(dbSet);

            if (orderBy != null)
                dbSet = orderBy(dbSet);

            return await dbSet.ToListAsync(cancellationToken);
        }

        public virtual Task<TEntity?> GetByIdAsync(
            Guid id,
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            if (include != null)
                dbSet = include(dbSet);

            return dbSet.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public virtual Task<TEntity?> GetFirstAsync(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
             bool ignoreRemovedItens = true,
            CancellationToken cancellationToken = default)
        {
            ValidateTenant();
            var dbSet = ignoreRemovedItens ? TenantAllNotRemoved() : DBSet.Where(e => e.TenantId == TenantId);
            if (where != null)
                dbSet = dbSet.Where(where);

            if (include != null)
                dbSet = include(dbSet);

            return dbSet.FirstOrDefaultAsync(cancellationToken);
        }
        public virtual Task<TEntity?> GetFirstNoTrakingAsync(
    Expression<Func<TEntity, bool>>? where = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
     bool ignoreRemovedItens = true, CancellationToken cancellationToken = default)
        {
            ValidateTenant();
            var dbSet = ignoreRemovedItens ? TenantAllNotRemoved() : DBSet.Where(e => e.TenantId == TenantId);
            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            if (include != null)
                dbSet = include(dbSet);

            return dbSet.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? where = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            return dbSet.CountAsync(cancellationToken);
        }
        public virtual int Count(
         Expression<Func<TEntity, bool>>? where = null,
         CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            return dbSet.Count();
        }

        public virtual async Task<IList<TEntity>> GetPagedAsync(
            int skip,
            int take,
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.Where(where);

            if (include != null)
                dbSet = include(dbSet);

            if (orderBy != null)
                dbSet = orderBy(dbSet);

            var a = dbSet.Skip(skip).Take(take);

            return await a.ToListAsync(cancellationToken);
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? where = null, CancellationToken cancellationToken = default)
        {
            ValidateTenant();
            var dbSet = DBSet.Where(e => e.TenantId == TenantId).AsQueryable();

            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            return dbSet.AnyAsync(cancellationToken);
        }
        public Task<bool> ExistsNotRemovedAsync(Expression<Func<TEntity, bool>>? where = null, CancellationToken cancellationToken = default)
        {
            var dbSet = TenantAllNotRemoved();

            if (where != null)
                dbSet = dbSet.AsNoTracking().Where(where);

            return dbSet.AnyAsync(cancellationToken);
        }

        public virtual async Task BulkInsert(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DBSet.AddRangeAsync(entities.Select(c => { c.CreatedAt = DateTime.Now; return c; }), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public virtual async Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            ValidateTenant();
            entity.CreatedAt = DateTime.Now;
            entity.TenantId = TenantId;

            DBSet.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ValidateTenant();
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.Now;
                entity.TenantId = TenantId;
            }

            DBSet.AddRange(entities);

            await _context.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public virtual async Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.UpdatedAt = DateTime.Now;
            DBSet.Update(entity);

            _context.Entry(entity).Member("CreatedAt").IsModified = false;
            _context.Entry(entity).Member(nameof(TenantId)).IsModified = false;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<bool> Update(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                entity.UpdatedAt = DateTime.Now;

            DBSet.UpdateRange(entities);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
                return false;

            return await Delete(entity, cancellationToken);
        }

        public virtual async Task<bool> Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.RemovedAt = DateTime.Now;

            DBSet.Update(entity);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                entity.RemovedAt = DateTime.Now;
            }

            DBSet.UpdateRange(entities);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> ForceDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
                return false;

            return await ForceDeleteAsync(entity, cancellationToken);
        }

        public virtual async Task<bool> ForceDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DBSet.Remove(entity);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> ForceDeleteAsync(IEnumerable<TEntity> entities, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            DBSet.RemoveRange(entities);

            if (saveChanges)
                return await _context.SaveChangesAsync(cancellationToken) > 0;

            return true;
        }

        private IQueryable<TEntity> AllNotRemoved()
        {
            return DBSet.Where(x => x.RemovedAt == null);

        }
        private IQueryable<TEntity> TenantAllNotRemoved()
        {
            ValidateTenant();
            return DBSet.Where(e => e.TenantId == TenantId)
                .Where(x => x.RemovedAt == null);


        }

        private void ValidateTenant()
        {
            if (TenantId == Guid.Empty || TenantId == default)
            {
                throw new InvalidTenantException("Invalid Tenant");
            }
        }


    }
}