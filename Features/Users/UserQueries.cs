using axon_final_api.Common;
using axon_final_api.Data;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Features.Users;

[Queries]
public partial class UserQueries
{
     
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> Users([Service] DataContext db) 
    {
        return db.Users.AsQueryable();
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Role> Roles([Service] DataContext db) 
    {
        return db.Roles.AsQueryable();
    }

    
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Permission> Permissions([Service] DataContext db) 
    {
        return db.Permissions.AsQueryable();
    }

    [Query<CallResult<UserResponse>>]
    public record GetUserById(long Id);  

}
