using Axon_Job_App.Common;
using Cai;
using Microsoft.AspNetCore.Authorization;

namespace Axon_Job_App.Features.Users;

public class UserMutation
{
    // User mutations
    [Mutation<CallResult<LoginResponse>>]
    public record Login(LoginRequest Input);
    
    [Mutation<CallResult<UserResponse>>]
    public record CreateUser(CreateUserRequest Input);

    [Mutation<CallResult<UserResponse>>] 
    public record UpdateUser(long Id, UpdateUserRequest Input);

    [Mutation<CallResult>]
    public record DeleteUser(long Id);

    // Role mutations
    [Mutation<CallResult<RoleResponse>>]
    public record CreateRole(CreateRoleRequest Input);

    [Mutation<CallResult<RoleResponse>>]
    public record UpdateRole(long Id, CreateRoleRequest Input);

    [Mutation<CallResult>]
    public record DeleteRole(long Id);

    // Permission mutations
    [Mutation<CallResult>]
    public record AssignPermissions(AssignPermissionsRequest Input);

    [Mutation<CallResult>]
    public record RevokePermissions(AssignPermissionsRequest Input);
}