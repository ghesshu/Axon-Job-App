using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.EntityFrameworkCore;
using Axon_Job_App.Common.Extensions;
using Axon_Job_App.Features.Jobs;
using Axon_Job_App.Services;

namespace Axon_Job_App.Features.Candidates;

public class CandidateHandler(AuthContext authContext)
{
       private async Task EnsureAuthenticated()
    {
        if (!await Task.FromResult(authContext.IsAuthenticated()))
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
    }
    public async Task<CallResult<CandidateResponse>> Handle(
        CandidateMutation.CreateCandidate command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated();

            var existingCandidate = await db.Candidates
                .FirstOrDefaultAsync(c => c.Email == command.Input.Email, ct);
            
            if (existingCandidate != null)
                return CallResult<CandidateResponse>.error("Candidate already exists");

            var candidate = new Candidate
            {
                Name = command.Input.Name,
                Email = command.Input.Email,
                Phone = command.Input.Phone,
                Skills = command.Input.Skills,
                Experience = command.Input.Experience
            };

            db.Candidates.Add(candidate);
            await db.SaveChangesAsync(ct);

            return CallResult<CandidateResponse>.ok(new CandidateResponse(
                candidate.Id,
                candidate.Name,
                candidate.Email,
                candidate.Phone,
                candidate.Skills,
                candidate.Experience,
                candidate.Verified,
                candidate.CreatedAt,
                candidate.UpdatedAt
            ), "Candidate created successfully");
        }
        catch (Exception e)
        {
            return CallResult<CandidateResponse>.error(e.Message);
        }
    }

    public async Task<CallResult<CandidateResponse>> Handle(
        CandidateMutation.UpdateCandidate command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated();

            var candidate = await db.Candidates.FindAsync([command.Id], ct);
            if (candidate == null)
                return CallResult<CandidateResponse>.error("Candidate not found");

            if (!string.IsNullOrEmpty(command.Input.Name))
                candidate.Name = command.Input.Name;

            if (!string.IsNullOrEmpty(command.Input.Email))
                candidate.Email = command.Input.Email;

            if (!string.IsNullOrEmpty(command.Input.Phone))
                candidate.Phone = command.Input.Phone;

            if (command.Input.Skills != null)
                candidate.Skills = command.Input.Skills;

            if (command.Input.Experience != null)
                candidate.Experience = command.Input.Experience;

            candidate.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            return CallResult<CandidateResponse>.ok(new CandidateResponse(
                candidate.Id,
                candidate.Name,
                candidate.Email,
                candidate.Phone,
                candidate.Skills,
                candidate.Experience,
                candidate.Verified,
                candidate.CreatedAt,
                candidate.UpdatedAt
            ), "Candidate updated successfully");
        }
        catch (Exception e)
        {
            return CallResult<CandidateResponse>.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        CandidateMutation.DeleteCandidate command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated();

            var candidate = await db.Candidates
                .Include(c => c.Assignments)
                .FirstOrDefaultAsync(c => c.Id == command.Id, ct);
            
            if (candidate == null)
                return CallResult.error("Candidate not found");

            // Check if candidate has active job assignments
            var activeAssignments = candidate.Assignments
                .Any(a => a.Status != AssignmentStatus.Terminated);

            if (activeAssignments)
                return CallResult.error("Candidate cannot be deleted with active job assignments");

            // Delete terminated assignments if any
            var terminatedAssignments = candidate.Assignments
                .Where(a => a.Status == AssignmentStatus.Terminated)
                .ToList();

            if (terminatedAssignments.Any())
            {
                db.JobAssignments.RemoveRange(terminatedAssignments);
                await db.SaveChangesAsync(ct);
            }

            // Delete the candidate
            db.Candidates.Remove(candidate);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Candidate and terminated assignments deleted successfully");
        }
        catch (Exception e)
        {
            
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        CandidateMutation.VerifyCandidate command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated();

            var candidate = await db.Candidates.FindAsync([command.Id], ct);
            if (candidate == null)
                return CallResult.error("Candidate not found");

            candidate.Verified = true;
            candidate.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Candidate verified successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        CandidateMutation.AssignCandidateToJob command, 
        DataContext db, 
        CancellationToken ct)
    {
        try
        {
            await EnsureAuthenticated();
            
            var candidateExists = await db.Candidates.AnyAsync(c => c.Id == command.CandidateId, ct);
            if (!candidateExists)
                return CallResult.error("Candidate not found");

            var jobExists = await db.Jobs.AnyAsync(j => j.Id == command.JobId, ct);
            if (!jobExists)
                return CallResult.error("Job not found");

            var assignment = new JobAssignment
            {
                CandidateId = command.CandidateId,
                JobId = command.JobId,
                Status = AssignmentStatus.Active,
            };

            db.JobAssignments.Add(assignment);
            await db.SaveChangesAsync(ct);

            return CallResult.ok("Candidate assigned to job successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }
}