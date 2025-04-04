using Axon_Job_App.Data;
using Axon_Job_App.Services;
using Cai;


namespace Axon_Job_App.Features.Candidates;

[Queries]
public partial class CandidateQueries
{
    [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 20)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Candidate> Candidates([Service] DataContext db, [Service] AuthContext authContext) 
    {
        if (!authContext.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
        return db.Candidates.AsQueryable();
    }
}