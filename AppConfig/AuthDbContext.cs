using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace FinBookeAPI.AppConfig;

public class AuthDbContext(
    DbContextOptions<AuthDbContext> options,
    IOptions<AuthDatabaseSettings> _settings
) : IdentityDbContext<DbUser>(options)
{
    //public DbSet<DbUser> Test { get; init; }
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
        //builder.Entity<DbUser>().ToCollection("random");
    }
}
