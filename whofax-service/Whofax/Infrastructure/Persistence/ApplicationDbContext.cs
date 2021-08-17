using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Whofax.Application.Common.Interfaces;
using Whofax.Domain.Common;
using Whofax.Domain.Entities.Identity;

namespace Whofax.Infrastructure.Persistence;

/// <summary>
/// Adding of migrations: add-migration migration_name -Context ApplicationDbContext -OutputDir Persistence\Migrations
/// </summary>
public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var dateTime = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = dateTime;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = dateTime;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
