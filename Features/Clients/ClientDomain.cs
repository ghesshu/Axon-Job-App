using System;

namespace axon_final_api.Features.Clients;

public record ClientResponse(
    long Id,
    string CompanyName,
    string? CompanyImage,
    string CompanyLocation,
    DateTime DateJoined,
    string VerificationStatus,
    int NumberOfAttendingCandidates,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateClientRequest(
    string CompanyName,
    string? CompanyImage,
    string CompanyLocation,
    VerificationStatus VerificationStatus = VerificationStatus.Pending
);

public record UpdateClientRequest(
    string? CompanyName,
    string? CompanyImage,
    string? CompanyLocation,
    VerificationStatus? VerificationStatus
);

public enum VerificationStatus
{
    Pending,
    Verified,
    Rejected,
    Suspended
}