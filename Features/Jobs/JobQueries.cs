using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Axon_Job_App.Features.Clients;
using Axon_Job_App.Services;
using Cai;
using JasperFx.Core.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Jobs;

[Queries]
public partial class JobQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Job> Jobs([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Jobs.AsQueryable();
    }

    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JobAssignment> JobAssignments([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.JobAssignments.AsQueryable();
    }
    
}