using System;
using axon_final_api.Features.Jobs;

namespace axon_final_api.Features.Clients;

public record JobResponse(
    long Id,
    long ClientId,
    string Title,
    string? TemporaryType,
    JobType JobType,
    JobStatus Status,
    PaymentType PaymentType,
    decimal PaymentAmount,
    string[] Duties,
    string[] Requirements,
    string JobHours,
    string Location,
    bool Published,
    DateTime StartDate,
    int NumberOfRoles,
    string? WorkingHours,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateJobRequest(
    long ClientId,
    string Title,
    string? TemporaryType,
    JobType JobType,
    PaymentType PaymentType,
    decimal PaymentAmount,
    string[] Duties,
    string[] Requirements,
    string JobHours,
    string Location,
    DateTime StartDate,
    int NumberOfRoles,
    string? WorkingHours = null
);

public record UpdateJobRequest(
    string? Title,
    string? TemporaryType,
    JobType? JobType,
    PaymentType? PaymentType,
    decimal? PaymentAmount,
    string[]? Duties,
    string[]? Requirements,
    string? JobHours,
    string? Location,
    bool? Published,
    DateTime? StartDate,
    int? NumberOfRoles,
    string? WorkingHours
);

public record AssignJobRequest(
    long JobId,
    long CandidateId,
    AssignmentStatus Status = AssignmentStatus.Active
);