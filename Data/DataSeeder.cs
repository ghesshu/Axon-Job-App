using axon_final_api.Features.Users;
using axon_final_api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Data;

public class DataSeeder
{
    public static async Task SeedData(DataContext context)
    {
        // Seed Permissions
        var permissions = Permissions.All.Select(p => new Permission
        {
            Name = p,
            Description = $"Permission to {p}"
        }).ToList();

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();  // Save permissions first

        // Create Admin Role with all permissions
        var adminRole = new Role
        {
            Name = "Admin",
            Description = "Administrator role with full access",
            RolePermissions = [.. permissions.Select(p => new RolePermission
            {
                Permission = p
            })]
        };

        // Create Client Role
        var clientRole = new Role
        {
            Name = "Client",
            Description = "Client role with limited access"
        };

        // Create Candidate Role
        var candidateRole = new Role
        {
            Name = "Candidate",
            Description = "Candidate role with basic access"
        };

        await context.Roles.AddRangeAsync(adminRole, clientRole, candidateRole);
        await context.SaveChangesAsync();  // Save roles before creating users

        // Create Admin User
        if (!await context.Users.AnyAsync(u => u.Email == "admin@axon.com"))
        {
            var adminUser = new User
            {
                Email = "admin@axon.com",
                RoleId = adminRole.Id
            };
            adminUser.SetPassword("Admin@123");

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}