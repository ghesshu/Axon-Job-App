namespace Axon_Job_App.Features.Candidates;

public record CreateCandidateRequest(
    string Name,
    string Email,
    string Phone,
    string[] Skills,
    string? Experience
);

public record UpdateCandidateRequest(
    string? Name,
    string? Email,
    string? Phone,
    string[]? Skills,
    string? Experience
);

public record CandidateResponse(
    long Id,
    string Name,
    string Email,
    string Phone,
    string[] Skills,
    string? Experience,
    bool Verified,
    DateTime CreatedAt,
    DateTime UpdatedAt
);