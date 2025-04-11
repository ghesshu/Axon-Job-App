using System;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

using Axon_Job_App.Helpers;
// using Axon_Job_App.Features.Jobs;

namespace Axon_Job_App.Data;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    static DataContext()
    {
        Batteries_V2.Init(); // Proper initialization for SQLitePCL
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCandidates(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureJobs(modelBuilder);
        ConfigureClients(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.FindProperty("CreatedAt") != null)
            {
                entityType.GetProperty("CreatedAt")?
                    .SetDefaultValueSql("CURRENT_TIMESTAMP");
            }

            if (entityType.FindProperty("UpdatedAt") != null)
            {
                entityType.GetProperty("UpdatedAt")?
                    .SetDefaultValueSql("CURRENT_TIMESTAMP");
            }
        }
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasTimestamps && 
                    (e.State == EntityState.Modified || e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            ((IHasTimestamps)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((IHasTimestamps)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Same logic as above for async
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasTimestamps && 
                    (e.State == EntityState.Modified || e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            ((IHasTimestamps)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((IHasTimestamps)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    

}
