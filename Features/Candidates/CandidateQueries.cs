using axon_final_api.Common;
using axon_final_api.Data;
using Cai;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Features.Candidates;

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