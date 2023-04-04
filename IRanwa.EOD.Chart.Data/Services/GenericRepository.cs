using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Generic repository.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="IGenericRepository&lt;TEntity&gt;" />
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWorkAsync unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public GenericRepository(IUnitOfWorkAsync unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    /// Returns entity.
    /// </returns>
    public async Task<EntityEntry<TEntity>> Add(TEntity entity)
    {
        return await unitOfWork.GetDBContext().Set<TEntity>().AddAsync(entity);
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    /// Returns entity.
    /// </returns>
    public EntityEntry<TEntity> Delete(int id)
    {
        var entity = FindById(id).Result;
        if (entity != null)
            return unitOfWork.GetDBContext().Set<TEntity>().Remove(entity);
        return null;
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>
    /// Returns entity.
    /// </returns>
    public EntityEntry<TEntity> Update(TEntity entity)
    {
        return unitOfWork.GetDBContext().Set<TEntity>().Update(entity);
    }

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    /// Returns entity.
    /// </returns>
    public async Task<TEntity> FindById(int id)
    {
        return await unitOfWork.GetDBContext().Set<TEntity>().FindAsync(id);
    }

    /// <summary>
    /// Gets the queryable.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="orderBy">The order by.</param>
    /// <returns>
    /// Returns entity list.
    /// </returns>
    public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
    {
        IQueryable<TEntity> query = unitOfWork.GetDBContext().Set<TEntity>();
        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return query.AsQueryable();
    }

    /// <summary>
    /// Gets the one.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>Get one by id.</returns>
    public TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        IQueryable<TEntity> query = unitOfWork.GetDBContext().Set<TEntity>();
        if (predicate != null)
            query = query.Where(predicate);
        return query.FirstOrDefault();
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>
    /// Returns list of entities.
    /// </returns>
    public IQueryable<TEntity> GetAll()
    {
        IQueryable<TEntity> query = unitOfWork.GetDBContext().Set<TEntity>();
        return query;
    }
}
