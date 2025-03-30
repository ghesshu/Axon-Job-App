using axon_final_api.Common;
using axon_final_api.Features.Clients;
using Cai;

namespace axon_final_api.Features.Jobs;

public class JobMutation
{
    [Mutation<CallResult<JobResponse>>]
    public record CreateJob(CreateJobRequest Input);

    [Mutation<CallResult<JobResponse>>]
    public record UpdateJob(long Id, UpdateJobRequest Input);

    [Mutation<CallResult>]
    public record DeleteJob(long Id);

    [Mutation<CallResult>]
    public record PublishJob(long Id);

    [Mutation<CallResult>]
    public record AssignJob(AssignJobRequest Input);

    [Mutation<CallResult>]
    public record UpdateAssignmentStatus(long JobId, long CandidateId, AssignmentStatus Status);
}