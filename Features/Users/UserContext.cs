using System;
using Axon_Job_App.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Data;

public partial class DataContext
{
    public DbSet<User> Users { get; set; } 

    public DbSet<Role> Roles { get; set;} 

    public DbSet<Permission> Permissions { get; set;}

    public DbSet<RolePermission> RolePermissions { get; set; } = null!;

    private static void ConfigureUsers(ModelBuilder modelBuilder)
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
    }
}
