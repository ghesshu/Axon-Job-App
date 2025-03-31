using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Features.Candidates;

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