using System.Threading;
using System.Threading.Tasks;
using axon_final_api.Common;
using axon_final_api.Data;
using Microsoft.EntityFrameworkCore;

namespace axon_final_api.Features.Candidates;

public class CandidateHandler
{
    public Task<CallResult<CandidateResponse>> Handle(CandidateMutation.CreateCandidate command, DataContext db, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CallResult<CandidateResponse>> Handle(CandidateMutation.UpdateCandidate command, DataContext db, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CallResult> Handle(CandidateMutation.DeleteCandidate command, DataContext db, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CallResult> Handle(CandidateMutation.VerifyCandidate command, DataContext db, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CallResult> Handle(CandidateMutation.AssignCandidateToJob command, DataContext db, CancellationToken ct)
        => throw new NotImplementedException();
}