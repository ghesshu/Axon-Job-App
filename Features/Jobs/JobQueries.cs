using axon_final_api.Common;
using axon_final_api.Data;
using axon_final_api.Features.Clients;
using Cai;
using JasperFx.Core.Reflection;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Features.Jobs;

[Queries]
public partial class JobQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Job> Jobs([Service] DataContext db) 
    {
        return db.Jobs.AsQueryable();
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobAssignment> JobAssignments([Service] DataContext db) 
    {
        return db.JobAssignments.AsQueryable();
    }
    
}