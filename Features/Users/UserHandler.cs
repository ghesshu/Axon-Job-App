using System.Threading;
using System.Threading.Tasks;
using axon_final_api.Common;
using axon_final_api.Data;
using Microsoft.AspNetCore.Identity;
using static BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using axon_final_api.Common.Extensions;

namespace axon_final_api.Features.Users;

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
    public Task<CallResult<RoleResponse>> Handle(UserMutation.CreateRole command, DataContext db, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    public Task<CallResult<RoleResponse>> Handle(UserMutation.UpdateRole command, DataContext db, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(UserMutation.DeleteRole command, DataContext db, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    // Permission handlers
    public Task<CallResult> Handle(UserMutation.AssignPermissions command, DataContext db, CancellationToken cancellationToken) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(UserMutation.RevokePermissions command, DataContext db, CancellationToken cancellationToken) 
        => throw new NotImplementedException();
}