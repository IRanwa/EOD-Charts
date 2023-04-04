using IRanwa.EOD.Chart.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IRanwa.EOD.Chart.Data;


/// <summary>
/// EOD db context.
/// </summary>
/// <seealso cref="IdentityDbContext&lt;ApplicationUser&gt;" />
public class EODDBContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EODDBContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public EODDBContext(DbContextOptions<EODDBContext> options) : base(options)
    {

    }

    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<ExchangeCode> ExchangeCodes { get; set; }
    public DbSet<ExchangeSymbol> ExchangeSymbols { get; set; }
    public DbSet<StockQuarterly> StockQuarterly { get; set; }
    public DbSet<StockAnnual> StockAnnual { get; set; }
    public DbSet<EODData> EODData { get; set; }
    public DbSet<EODLiveData> EODLiveData { get; set; }
    public DbSet<AspNetForgetPassword> AspNetForgetPassword { get; set; }
    public DbSet<AspNetUsersHistory> AspNetUsersHistory { get; set; }
    public DbSet<AspNetEmailVerifications> AspNetEmailVerifications { get; set; }

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.</param>
    /// <remarks>
    /// <para>
    /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
    /// then this method will not be run.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information.
    /// </para>
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private void SeedData(ModelBuilder modelBuilder)
    {
        #region Identity Roles
        var identityRoles = new List<IdentityRole>();
        foreach (RoleTypes role in Enum.GetValues(typeof(RoleTypes)))
            identityRoles.Add(
                new IdentityRole() { Id = role.GetHashCode().ToString(), Name = role.ToString(), NormalizedName = role.ToString().ToUpper() }
            );

        modelBuilder.Entity<IdentityRole>().HasData(identityRoles);
        #endregion
    }
}