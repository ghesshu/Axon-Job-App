using System;

namespace Axon_Job_App.Helpers;

public static class Permissions
{
    public static readonly string[] All =
    [
        // User Management
        "ReadUsers", "CreateUser", "UpdateUser", "DeleteUser",
        
        // Role Management
        "ReadRoles", "CreateRole", "UpdateRole", "DeleteRole",
        
        // Permission Management
        "ReadPermissions",
        
        // Add more permissions as needed
    ];
}
