using System;
using Axon_Job_App.Features.Jobs;

namespace Axon_Job_App.Features.Clients;

public record JobResponse(
    long Id,
    long ClientId,
    string Title,
    JobType JobType,
    JobStatus Status,
    PaymentType PaymentType,
    decimal SalaryPerAnnum,
    string[] Duties,
    string[] Requirements,
    string JobHours,
    string Location,
    DateTime StartDate,
    int NumberOfRoles,
    bool Published,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateJobRequest(
    long ClientId,
    string Title,
    JobType JobType,
    PaymentType PaymentType,
    decimal SalaryPerAnnum,
    string[] Duties,
    string[] Requirements,
    string JobHours,
    string Location,
    DateTime StartDate,
    int NumberOfRoles,
    bool Published = false
);

public record UpdateJobRequest(
    string? Title,
    JobType? JobType,
    PaymentType? PaymentType,
    decimal? SalaryPerAnnum,
    string[]? Duties,
    string[]? Requirements,
    string? JobHours,
    string? Location,
    DateTime? StartDate,
    int? NumberOfRoles,
    bool? Published
);
public record AssignJobRequest(
    long JobId,
    long CandidateId,
    AssignmentStatus Status = AssignmentStatus.Active
);