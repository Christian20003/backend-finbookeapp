using System.Linq.Expressions;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FinBookeAPI.AppConfig;

public class AuthDbContext(
    DbContextOptions<AuthDbContext> options,
    IOptions<AuthDatabaseSettings> _settings
) : IdentityDbContext<UserDatabase>(options)
{
    public DbSet<RefreshToken> RefreshToken { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var database = _settings.Value;
        var mongoClient = new MongoClient(database.ConnectionString);
        optionsBuilder.UseMongoDB(mongoClient, database.DatabaseName);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RefreshToken>().ToCollection("authentication");
    }

    /// <summary>
    /// This method searches for a particular <c>RefreshToken</c> in the authentication database. The first
    /// element that fulfilled the criterion (<c>predicate</c>) will be returned, otherwise <c>null</c>.
    /// </summary>
    /// <param name="predicate">
    /// A function which implements the criterion to be searched for. The return-type must be a <c>bool</c>.
    /// </param>
    /// <returns>
    /// The first <c>RefreshToken</c> instance that met the criterion, otherwise <c>null</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the requested search operation has been cancelled.
    /// </exception>
    public virtual Task<RefreshToken?> FindRefreshToken(
        Expression<Func<RefreshToken, bool>> predicate
    )
    {
        return RefreshToken.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// This method adds a new instance of <c>RefreshToken</c> to the authentication database.
    /// </summary>
    /// <param name="token">
    /// The instance that should be added to the database.
    /// </param>
    /// <returns>
    /// The instance that has been added to the database.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the requested add operation has been cancelled.
    /// </exception>
    public virtual async Task<RefreshToken> AddRefreshToken(RefreshToken token)
    {
        var result = await RefreshToken.AddAsync(token);
        return result.Entity;
    }
}
