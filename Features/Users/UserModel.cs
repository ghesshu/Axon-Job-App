using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using axon_final_api.Helpers;

namespace axon_final_api.Features.Users;

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

    public virtual Role? Role { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void SetPassword(string password)
    {
        Password = BCrypt.Net.BCrypt.HashPassword(password);
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
    [ForeignKey(nameof(Role))]
    public long RoleId { get; set; }

    [ForeignKey(nameof(Permission))]
    [MaxLength(100)]
    public string PermissionName { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}
