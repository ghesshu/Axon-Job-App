using Axon_Job_App.Common;
using Cai;

namespace Axon_Job_App.Features.Candidates;

public class CandidateMutation
{
    [Mutation<CallResult<CandidateResponse>>]
    public record CreateCandidate(CreateCandidateRequest Input);

    [Mutation<CallResult<CandidateResponse>>]
    public record UpdateCandidate(long Id, UpdateCandidateRequest Input);

    [Mutation<CallResult>]
    public record DeleteCandidate(long Id);

    [Mutation<CallResult>]
    public record VerifyCandidate(long Id);

    [Mutation<CallResult>]
    public record AssignCandidateToJob(long CandidateId, long JobId);
}