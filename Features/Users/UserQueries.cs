using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Axon_Job_App.Services;
using Cai;


namespace Axon_Job_App.Features.Users;

[Queries]
public partial class UserQueries
{

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> Users([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Users.AsQueryable();
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Role> Roles([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Roles.AsQueryable();
    }

    
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Permission> Permissions([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Permissions.AsQueryable();
    }

    [Query<CallResult<UserResponse>>]
    public record GetUserById(long Id); 

}
