using System;

namespace Axon_Job_App.Helpers;

public static class Permissions
{
    public static readonly string[] All =
    [
        // User Management
        "ReadUsers", "CreateUser", "UpdateUser",
        
        // Role Management
        "ReadRoles", "CreateRole", "UpdateRole",

        //Custom
        "createUser","createClient","seeAnalysis",
        "deleteClient","deleteCandidate","verifyClient"

    ];
}
