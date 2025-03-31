using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.AspNetCore.Identity;
using static BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using Axon_Job_App.Common.Extensions;

namespace Axon_Job_App.Features.Users;

public class UserHandler()
{     
    // private readonly AuthContext authContext = _authContext;

    // User handlers
    public async Task<CallResult<LoginResponse>> Handle(UserMutation.Login command, DataContext db,ILogger<UserHandler> logger, CancellationToken cancellationToken)
    {
       try
       {
         var user = await db.Users
            .Include(u => u.Role)
            .ThenInclude(r => r!.RolePermissions)
            .FirstOrDefaultAsync(u => u.Email == command.Input.Email, cancellationToken);

        if (user == null || !Verify(command.Input.Password, user.Password))
            return CallResult<LoginResponse>.error("Invalid credentials");

        var permissions = user.Role?.RolePermissions
            .Select(rp => rp.PermissionName) ?? [];

        // var token = authContext.GenerateToken(
        //     user.Id,
        //     user.Email,
        //     user.Role?.Name ?? "User",
        //     permissions);

        return CallResult<LoginResponse>.ok(new LoginResponse(
            user.Id,
            user.Email,
            user.Role?.Name ?? "User",
            permissions,
            // token
            "Hello-world"
            ),
            "Login successful");
       }
       catch (Exception e)
       {
            logger.logCommandError(command,e);
            return CallResult<LoginResponse>.error(e.Message);
       }
    }
        
    public async Task<CallResult<UserResponse>> Handle(
        UserMutation.CreateUser command, 
        DataContext db,
        ILogger<UserHandler> logger, 
        CancellationToken cancellationToken)
    {
       try
       {
        //  await authContext.CheckRole(db, "Admin");

        var existingUser = await db.Users
            .FirstOrDefaultAsync(u => u.Email == command.Input.Email, cancellationToken);
        
        if (existingUser != null)
            return CallResult<UserResponse>.error("User already exists");

        var user = new User
        {
            Email = command.Input.Email,
            RoleId = command.Input.RoleId
        };
        user.SetPassword(command.Input.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync(cancellationToken);

        return CallResult<UserResponse>.ok("User Created Successfully");
       }
       catch (Exception e)
       {
            logger.logCommandError(command,e);
            return CallResult<UserResponse>.error(e.Message);
       }
    }

    public async Task<CallResult<UserResponse>> Handle(
    UserMutation.UpdateUser command, 
    DataContext db, 
    ILogger<UserHandler> logger, 
    CancellationToken cancellationToken)
    {
       try
       {
        //  await authContext.CheckRole(db, "Admin");

        var user = await db.Users.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken);
        if (user == null)
            return CallResult<UserResponse>.error("User not found");

        if (!string.IsNullOrEmpty(command.Input.Email))
            user.Email = command.Input.Email;

        if (!string.IsNullOrEmpty(command.Input.Password))
            user.SetPassword(command.Input.Password);

        if (command.Input.RoleId.HasValue)
            user.RoleId = command.Input.RoleId.Value;

        await db.SaveChangesAsync(cancellationToken);

        return CallResult<UserResponse>.ok("User Updated Successfully");
       }
       catch (Exception e)
       {
            logger.logCommandError(command,e);
            return CallResult<UserResponse>.error(e.Message);
       }
    }

    public async Task<CallResult> Handle(
        UserMutation.DeleteUser command, 
        DataContext db, 
        ILogger<UserHandler> logger, 
        CancellationToken cancellationToken)
    {
       try
       {
        // await authContext.CheckRole(db, "Admin");

         var user = await db.Users.FindAsync([command.Id], cancellationToken: cancellationToken);
        if (user == null)
            return CallResult.error("User not found");

        db.Users.Remove(user);
        await db.SaveChangesAsync(cancellationToken);

        return CallResult.ok("User Deleted Successfully");
       }
       catch (Exception e)
       {
            logger.logCommandError(command,e);
            return CallResult.error(e.Message);
       }
    }

        // Role handlers
    public async Task<CallResult<RoleResponse>> Handle(
        UserMutation.CreateRole command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            var existingRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Name == command.Input.Name, cancellationToken);
            
            if (existingRole != null)
                return CallResult<RoleResponse>.error("Role already exists");

            var role = new Role
            {
                Name = command.Input.Name,
                Description = command.Input.Description,
                IsActive = true
            };

            db.Roles.Add(role);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<RoleResponse>.ok(new RoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsActive,
                command.Input.Permissions,
                role.CreatedAt,
                role.UpdatedAt
            ), "Role created successfully");
        }
        catch (Exception e)
        {
            return CallResult<RoleResponse>.error(e.Message);
        }
    }

    public async Task<CallResult<RoleResponse>> Handle(
        UserMutation.UpdateRole command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            var role = await db.Roles.FindAsync([command.Id], cancellationToken);
            if (role == null)
                return CallResult<RoleResponse>.error("Role not found");

            if (!string.IsNullOrEmpty(command.Input.Name))
                role.Name = command.Input.Name;

            if (!string.IsNullOrEmpty(command.Input.Description))
                role.Description = command.Input.Description;

            role.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<RoleResponse>.ok(new RoleResponse(
                role.Id,
                role.Name,
                role.Description,
                role.IsActive,
                command.Input.Permissions,
                role.CreatedAt,
                role.UpdatedAt
            ), "Role updated successfully");
        }
        catch (Exception e)
        {
            return CallResult<RoleResponse>.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        UserMutation.DeleteRole command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            var role = await db.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);
            
            if (role == null)
                return CallResult.error("Role not found");

            if (role.Users.Any())
                return CallResult.error("Cannot delete role with assigned users");

            db.Roles.Remove(role);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult.ok("Role deleted successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    // Permission handlers
    public async Task<CallResult> Handle(
        UserMutation.AssignPermissions command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            var role = await db.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == command.Input.RoleId, cancellationToken);
            
            if (role == null)
                return CallResult.error("Role not found");

            var existingPermissions = role.RolePermissions
                .Select(rp => rp.PermissionName)
                .ToHashSet();

            var newPermissions = command.Input.PermissionNames
                .Where(p => !existingPermissions.Contains(p))
                .Select(p => new RolePermission
                {
                    RoleId = command.Input.RoleId,
                    PermissionName = p
                });

            db.RolePermissions.AddRange(newPermissions);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult.ok("Permissions assigned successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        UserMutation.RevokePermissions command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            var permissionsToRemove = await db.RolePermissions
                .Where(rp => rp.RoleId == command.Input.RoleId &&
                    command.Input.PermissionNames.Contains(rp.PermissionName))
                .ToListAsync(cancellationToken);
            
            if (!permissionsToRemove.Any())
                return CallResult.error("No matching permissions found");

            db.RolePermissions.RemoveRange(permissionsToRemove);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult.ok("Permissions revoked successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
}