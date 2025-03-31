using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Candidates;

[Queries]
public partial class CandidateQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Candidate> Candidates([Service] DataContext db) 
    {
        return db.Candidates.AsQueryable();
    }
}