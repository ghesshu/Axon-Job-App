using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.EntityFrameworkCore;
using Axon_Job_App.Features.Clients;
using Axon_Job_App.Services;


namespace Axon_Job_App.Features.Jobs;

public class JobHandler(AuthContext authContext)
{
    public async Task EnsureAuthenticated(AuthContext authContext)
    {
        if (!await Task.FromResult(authContext.IsAuthenticated()))
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
    }

    private static JobResponse MapToResponse(Job job) => new(
        job.Id,
        job.ClientId,
        job.Title,
        job.JobType,
        job.Status,
        job.PaymentType,
        job.SalaryPerAnnum,
        job.Duties,
        job.Requirements,
        job.JobHours,
        job.Location,
        job.StartDate,
        job.NumberOfRoles,
        job.Published,
        job.CreatedAt,
        job.UpdatedAt
    );

    public async Task<CallResult<JobResponse>> Handle(
        JobMutation.CreateJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            if (!await db.Clients.AnyAsync(c => c.Id == command.Input.ClientId, ct))
                return CallResult<JobResponse>.error("Client not found");

            var job = new Job
            {
                ClientId = command.Input.ClientId,
                Title = command.Input.Title,
                JobType = command.Input.JobType,
                PaymentType = command.Input.PaymentType,
                SalaryPerAnnum = command.Input.SalaryPerAnnum,
                Duties = command.Input.Duties[..Math.Min(command.Input.Duties.Length, 4)],
                Requirements = command.Input.Requirements[..Math.Min(command.Input.Requirements.Length, 4)],
                JobHours = command.Input.JobHours,
                Location = command.Input.Location,
                StartDate = command.Input.StartDate,
                NumberOfRoles = command.Input.NumberOfRoles
            };

            await db.Jobs.AddAsync(job, ct);
            await db.SaveChangesAsync(ct);

            return CallResult<JobResponse>.ok(MapToResponse(job), "Job created successfully");
        }
        catch (Exception e)
        {
            return CallResult<JobResponse>.error(e.Message);
        }
    }

    public async Task<CallResult<JobResponse>> Handle(
        JobMutation.UpdateJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            var job = await db.Jobs.FindAsync([command.Id], ct);
            if (job == null)
                return CallResult<JobResponse>.error("Job not found");

            // Update properties
            if (!string.IsNullOrEmpty(command.Input.Title))
                job.Title = command.Input.Title;
            
            if (command.Input.JobType.HasValue)
                job.JobType = command.Input.JobType.Value;
            
            if (command.Input.PaymentType.HasValue)
                job.PaymentType = command.Input.PaymentType.Value;
            
            if (command.Input.SalaryPerAnnum.HasValue)
                job.SalaryPerAnnum = command.Input.SalaryPerAnnum.Value;
            
            if (command.Input.Duties != null)
                job.Duties = command.Input.Duties[..Math.Min(command.Input.Duties.Length, 4)];
            
            if (command.Input.Requirements != null)
                job.Requirements = command.Input.Requirements[..Math.Min(command.Input.Requirements.Length, 4)];
            
            if (!string.IsNullOrEmpty(command.Input.JobHours))
                job.JobHours = command.Input.JobHours;
            
            if (!string.IsNullOrEmpty(command.Input.Location))
                job.Location = command.Input.Location;
            
            if (command.Input.StartDate.HasValue)
                job.StartDate = command.Input.StartDate.Value;
            
            if (command.Input.NumberOfRoles.HasValue)
                job.NumberOfRoles = command.Input.NumberOfRoles.Value;
            
            if (command.Input.Published.HasValue)
            {
                job.Published = command.Input.Published.Value;
                job.Status = command.Input.Published.Value ? JobStatus.Published : JobStatus.Draft;
            }

            job.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            return CallResult<JobResponse>.ok(MapToResponse(job), "Job updated successfully");
        }
        catch (Exception e)
        {
            return CallResult<JobResponse>.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.DeleteJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            // Check existence and assignments in single query
            var jobInfo = await db.Jobs
                .Where(j => j.Id == command.Id)
                .Select(j => new { j.Id, AssignmentCount = j.Assignments.Count })
                .FirstOrDefaultAsync(ct);

            if (jobInfo == null)
                return CallResult.error("Job not found");

            if (jobInfo.AssignmentCount > 0)
                return CallResult.error("Cannot delete job with existing assignments");

            // Use ExecuteDelete for better performance
            await db.Jobs
                .Where(j => j.Id == command.Id)
                .ExecuteDeleteAsync(ct);

            return CallResult.ok("Job deleted successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        JobMutation.PublishJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            var updated = await db.Jobs
                .Where(j => j.Id == command.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(j => j.Published, true)
                    .SetProperty(j => j.Status, JobStatus.Published)
                    .SetProperty(j => j.UpdatedAt, DateTime.UtcNow),
                ct);

            if (updated == 0)
                return CallResult.error("Job not found");

            return CallResult.ok("Job published successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
    public async Task<CallResult> Handle(
        JobMutation.AssignJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            var job = await db.Jobs
                .Include(j => j.Assignments)
                .FirstOrDefaultAsync(j => j.Id == command.Input.JobId, ct);

            if (job == null)
                return CallResult.error("Job not found");

            if (job.Assignments.Count >= job.NumberOfRoles)
                return CallResult.error("Job has reached maximum number of assignments");

            var candidateExists = await db.Candidates.AnyAsync(c => c.Id == command.Input.CandidateId, ct);
            if (!candidateExists)
                return CallResult.error("Candidate not found");

            var assignment = new JobAssignment
            {
                JobId = command.Input.JobId,
                CandidateId = command.Input.CandidateId,
                Status = command.Input.Status
            };

            db.JobAssignments.Add(assignment);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Job assigned successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
    public async Task<CallResult> Handle(
        JobMutation.UpdateAssignmentStatus command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            var assignment = await db.JobAssignments
                .FirstOrDefaultAsync(a => 
                    a.JobId == command.JobId && 
                    a.CandidateId == command.CandidateId, ct);

            if (assignment == null)
                return CallResult.error("Assignment not found");

            assignment.Status = command.Status;
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Assignment status updated successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(JobMutation.DeleteAssignment command, DataContext db, CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated(authContext);

            var assignment = await db.JobAssignments
                .FirstOrDefaultAsync(a => 
                    a.JobId == command.JobId && 
                    a.CandidateId == command.CandidateId, ct);
            
            if(assignment == null)
            {
                return CallResult.error("Assignment not found");
            }

            db.JobAssignments.Remove(assignment);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Assignment deleted successfully");
        } catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
}