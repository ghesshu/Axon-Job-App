using System;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using axon_final_api.Features.Users;
using axon_final_api.Helpers;
using axon_final_api.Features.Jobs;
using axon_final_api.Features.Clients;
using axon_final_api.Features.Candidates;

namespace axon_final_api.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    static DataContext()
    {
        Batteries_V2.Init(); // Proper initialization for SQLitePCL
    }

    // Add DBSet for operations 

    public DbSet<User> Users { get; set; } 

    public DbSet<Role> Roles { get; set;} 

    public DbSet<Permission> Permissions { get; set;}

    public DbSet<RolePermission> RolePermissions { get; set; } = null!;

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<JobAssignment> JobAssignments { get; set; } = null!;

    public DbSet<Candidate> Candidates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User -> Role (Many-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Role -> Permission (Many-to-Many through RolePermission)
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionName });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionName);



        // Configure Client-Job relationship (One-to-Many)
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Jobs)
            .WithOne(j => j.Client)
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict if you prefer

        // Configure Job-Candidate many-to-many via JobAssignment
        modelBuilder.Entity<JobAssignment>()
            .HasKey(ja => new { ja.JobId, ja.CandidateId }); // Composite primary key

        modelBuilder.Entity<JobAssignment>()
            .HasOne(ja => ja.Job)
            .WithMany(j => j.Assignments)
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict

        modelBuilder.Entity<JobAssignment>()
            .HasOne(ja => ja.Candidate)
            .WithMany(c => c.Assignments)
            .HasForeignKey(ja => ja.CandidateId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict

        modelBuilder.Entity<Candidate>()
            .HasMany(c => c.Assignments)
            .WithOne(a => a.Candidate)
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);

        
         // Add Candidate DbSet
    

        // Optional: Configure enum conversions if needed
        modelBuilder.Entity<Job>()
            .Property(j => j.JobType)
            .HasConversion<string>();

        modelBuilder.Entity<Job>()
            .Property(j => j.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Job>()
            .Property(j => j.PaymentType)
            .HasConversion<string>();

        modelBuilder.Entity<JobAssignment>()
            .Property(ja => ja.Status)
            .HasConversion<string>();

        



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
