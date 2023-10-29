using Core.Entities.Bases;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories.Bases
{
    public interface IBaseEntityRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetDbSet(bool ignoreRemovedItens = true);
        Task<IEnumerable<TEntity>> GetMany(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(
            Guid id,
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetFirstAsync(
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
             bool ignoreRemovedItens = true, CancellationToken cancellationToken = default);
        Task<TEntity?> GetFirstNoTrakingAsync(
    Expression<Func<TEntity, bool>>? where = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
     bool ignoreRemovedItens = true, CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? where = null,
            CancellationToken cancellationToken = default);

        int Count(
    Expression<Func<TEntity, bool>>? where = null,
    CancellationToken cancellationToken = default);


        Task<IList<TEntity>> GetPagedAsync(
            int skip,
            int take,
            Expression<Func<TEntity, bool>>? where = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? where = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsNotRemovedAsync(Expression<Func<TEntity, bool>>? where = null, CancellationToken cancellationToken = default);

        Task<TEntity> Insert(TEntity entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task BulkInsert(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> Update(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<bool> Delete(Guid id, CancellationToken cancellationToken = default);

        Task<bool> Delete(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<bool> ForceDeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ForceDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<bool> ForceDeleteAsync(IEnumerable<TEntity> entities, bool saveChanges = true, CancellationToken cancellationToken = default);
    }
}
