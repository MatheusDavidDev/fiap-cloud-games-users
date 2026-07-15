using FCG.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.Infra;

public class FcgUsersDbContext : DbContext
{

    public FcgUsersDbContext(DbContextOptions<FcgUsersDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FcgUsersDbContext).Assembly);
    }

}
