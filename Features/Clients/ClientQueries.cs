using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Axon_Job_App.Services;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Clients;

[Queries]
public partial class ClientQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Client> Clients([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Clients.AsQueryable();
    }

}