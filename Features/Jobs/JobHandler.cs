using axon_final_api.Common;
using axon_final_api.Data;
using axon_final_api.Features.Clients;

namespace axon_final_api.Features.Jobs;

public class JobHandler
{
    public Task<CallResult<JobResponse>> Handle(JobMutation.CreateJob command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult<JobResponse>> Handle(JobMutation.UpdateJob command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(JobMutation.DeleteJob command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(JobMutation.PublishJob command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(JobMutation.AssignJob command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(JobMutation.UpdateAssignmentStatus command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();
}