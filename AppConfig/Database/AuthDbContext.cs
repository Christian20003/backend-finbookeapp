using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Token;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FinBookeAPI.AppConfig.Database;

/// <summary>
/// This class models the context for the authentication database.
/// </summary>
/// <param name="options">
/// The options to be used by a DbContext.
/// </param>
/// <param name="_settings">
/// The settings including all necessary information from the <c>appsettings.json</c> file.
/// </param>
public class AuthDbContext(
    DbContextOptions<AuthDbContext> options,
    IOptions<AuthDatabaseSettings> _settings
) : IdentityDbContext<UserAccount>(options)
{
    public DbSet<JwtToken> TokenCollection { get; init; }

    /// <summary>
    /// This function configures the authentication database by reading all specified configurations
    /// in the <c>appsettings.json</c> file.
    /// </summary>
    /// <param name="optionsBuilder">
    /// API to configure the DbContext for EF-Core.
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var database = _settings.Value;
        var mongoClient = new MongoClient(database.ConnectionString);
        optionsBuilder.UseMongoDB(mongoClient, database.DatabaseName);
    }

    /// <summary>
    /// This function creates all specified tables / collections in the database.
    /// </summary>
    /// <param name="builder">
    ///  API to model the shape of all entities in this database.
    /// </param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<JwtToken>().ToCollection("InvalidTokens");
    }
}
