using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Axon_Job_App.Features.Clients;

namespace Axon_Job_App.Features.Jobs;

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