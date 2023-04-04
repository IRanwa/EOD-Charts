using System.Security.Principal;

namespace IRanwa.EOD.Chart.Data;

/// <summary>
/// Unit of work async.
/// </summary>
/// <seealso cref="IUnitOfWorkAsync" />
/// <seealso cref="IDisposable" />
public class UnitOfWorkAsync : IUnitOfWorkAsync, IDisposable
{
    /// <summary>
    /// The context
    /// </summary>
    public EODDBContext context;

    /// <summary>
    /// The repositories asynchronous
    /// </summary>
    private Dictionary<Type, object> repositoriesAsync;

    /// <summary>
    /// The disposed
    /// </summary>
    private bool disposed;

    /// <summary>
    /// The user
    /// </summary>
    private readonly IPrincipal user;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWorkAsync"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="user">The user.</param>
    public UnitOfWorkAsync(EODDBContext context, IPrincipal user)
    {
        this.context = context;
        this.user = user;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    public void Dispose(bool isDisposing)
    {
        if (!disposed && isDisposing)
            context.Dispose();
        disposed = true;
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    /// <returns>
    /// Returns DB Context.
    /// </returns>
    public EODDBContext GetDBContext()
    {
        return context;
    }

    /// <summary>
    /// Gets the generic repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>
    /// Returns entity list.
    /// </returns>
    public IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : class
    {
        if (repositoriesAsync == null)
            repositoriesAsync = new Dictionary<Type, object>();
        var type = typeof(TEntity);
        if (!repositoriesAsync.ContainsKey(type))
            repositoriesAsync.Add(type, new GenericRepository<TEntity>(this));
        return (IGenericRepository<TEntity>)repositoriesAsync[type];
    }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns>
    /// Returns saved count.
    /// </returns>
    public int SaveChanges()
    {
        var addingEntries = context.ChangeTracker.Entries().Where(entry => 
            entry.Entity is EntityBase && 
            entry.State == Microsoft.EntityFrameworkCore.EntityState.Added).ToList();
        foreach(var entry in addingEntries)
        {
            ((EntityBase)entry.Entity).CreatedDateTime = DateTime.UtcNow;
            ((EntityBase)entry.Entity).CreatedUser = user.Identity == null ? null : user.Identity?.Name;
        }

        var updatingEntries = context.ChangeTracker.Entries().Where(entry =>
            entry.Entity is EntityBase &&
            entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified ||
            entry.State == Microsoft.EntityFrameworkCore.EntityState.Deleted).ToList();
        foreach (var entry in updatingEntries)
        {
            ((EntityBase)entry.Entity).ModifiedDateTime = DateTime.UtcNow;
            ((EntityBase)entry.Entity).ModifiedUser = user.Identity == null ? null : user.Identity?.Name;
        }

        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            var loginUser = context.ApplicationUser.FirstOrDefault(applicationUser => applicationUser.Id == user.Identity.Name);
            if(loginUser != null)
                loginUser.LastActive = DateTime.UtcNow;
        }
        return context.SaveChanges();
    }
}
