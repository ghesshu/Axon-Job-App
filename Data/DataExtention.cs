using System;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Data;

public static class DataExtention
{

      public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        
         // Migrate database
        dbContext.Database.Migrate();
        
        // Seed data only if database is empty
        if (!dbContext.Users.Any() && !dbContext.Roles.Any() && !dbContext.Permissions.Any())
        {
            DataSeeder.SeedData(dbContext).Wait();
        }
    }

}
