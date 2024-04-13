#region properties

using Microsoft.EntityFrameworkCore;
using TokenBased_Authentication.Domain.Entities.Account;
namespace TokenBased_Authentication.Infrastructure.ApplicationDbContext;

#endregion

public class TokenBased_AuthenticationDbContext : DbContext
{
    #region Ctor

    public TokenBased_AuthenticationDbContext(DbContextOptions<TokenBased_AuthenticationDbContext> options)
           : base(options)
    {

    }

    #endregion

    #region Entity

    #region User

    public DbSet<User> Users { get; set; }

    #endregion

    #endregion

    #region OnConfiguring

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
