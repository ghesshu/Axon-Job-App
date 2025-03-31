using System.ComponentModel.DataAnnotations;

namespace Axon_Job_App.Features.Users;

// Login DTOs
public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

public record LoginResponse(
    long UserId,
    string Email,
    string Role,
    IEnumerable<string> Permissions,
    string Token
);

// User Management DTOs
public record CreateUserRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password,
    [Required] long RoleId
);

public record UpdateUserRequest(
    [EmailAddress] string? Email,
    string? Password,
    long? RoleId
);

public record UserResponse(
    long Id,
    string Email,
    long RoleId,
    string RoleName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

// Role Management DTOs
public record CreateRoleRequest(
    [Required] string Name,
    string? Description,
    IEnumerable<string> Permissions
);

public record RoleResponse(
    long Id,
    string Name,
    string? Description,
    bool IsActive,
    IEnumerable<string> Permissions,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

// Permission DTOs
public record PermissionResponse(
    string Name,
    string? Description
);

// User-Role-Permission Query DTOs
public record UsersByRoleResponse(
    long RoleId,
    string RoleName,
    IEnumerable<UserResponse> Users
);

public record UserWithPermissionsResponse(
    UserResponse User,
    IEnumerable<PermissionResponse> Permissions
);

public record AssignPermissionsRequest(
    long RoleId,
    IEnumerable<string> PermissionNames
);

