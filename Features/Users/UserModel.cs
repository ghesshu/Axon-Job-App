using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Axon_Job_App.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Axon_Job_App.Data;

public partial class DataContext
{
    public DbSet<User> Users { get; set; } 

    public DbSet<Role> Roles { get; set;} 

    public DbSet<Permission> Permissions { get; set;}

    public DbSet<RolePermission> RolePermissions { get; set; } = null!;

}
public class User : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [ForeignKey(nameof(Role))]
    public long RoleId { get; set; }

    public virtual Role? Role { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void SetPassword(string password)
    {
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    public class TypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

public class Role  : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = [];
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    
    public class TypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // builder.HasOne(u => u.Role)
            //     .WithMany(r => r.Users)
            //     .HasForeignKey(u => u.RoleId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

public class Permission 
{
    [Key]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}

public class RolePermission
{
    public long Id { get; set; }
    // [ForeignKey(nameof(Role))]
    public long RoleId { get; set; }
    
    // [ForeignKey(nameof(Permission))]
    [MaxLength(100)]
    public string PermissionName { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
    
    public class TypeConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionName });
            
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId); 
            
            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionName);
        }
    }
}
