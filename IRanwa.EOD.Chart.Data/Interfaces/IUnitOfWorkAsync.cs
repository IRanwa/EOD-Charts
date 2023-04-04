namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Unit of work async.
/// </summary>
public interface IUnitOfWorkAsync
{
    /// <summary>
    /// Gets the database context.
    /// </summary>
    /// <returns>Returns DB Context.</returns>
    EODDBContext GetDBContext();

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns>Returns saved count.</returns>
    int SaveChanges();

    /// <summary>
    /// Gets the generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>Returns entity list.</returns>
    IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : class;
}
