using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Generic repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns entity.</returns>
    public Task<EntityEntry<TEntity>> Add(TEntity entity);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns entity.</returns>
    public EntityEntry<TEntity> Update(TEntity entity);

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns entity.</returns>
    public EntityEntry<TEntity> Delete(int id);

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns entity.</returns>
    public Task<TEntity> FindById(int id);

    /// <summary>
    /// Gets the queryable.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="orderBy">The order by.</param>
    /// <returns>Returns entity list.</returns>
    public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

    /// <summary>
    /// Gets the one.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>Returns entity.</returns>
    TEntity GetOne(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>Returns list of entities.</returns>
    IQueryable<TEntity> GetAll();
}
