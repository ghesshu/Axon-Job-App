using axon_final_api.Common;
using Cai;

namespace axon_final_api.Features.Candidates;

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