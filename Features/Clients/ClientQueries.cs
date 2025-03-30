using axon_final_api.Common;
using axon_final_api.Data;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Features.Clients;

[Queries]
public partial class ClientQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Client> Clients([Service] DataContext db) 
    {
        return db.Clients.AsQueryable();
    }

}