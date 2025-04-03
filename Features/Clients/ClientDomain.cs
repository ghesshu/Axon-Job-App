using System;

namespace Axon_Job_App.Features.Clients;

public record ClientResponse(
    long Id,
    string CompanyName,
    string CeoFirstName,
    string CeoLastName,
    string? JobTitle,
    string CompanyEmail,
    string CompanyPhone,
    string CompanyAddress,
    string PostalCode,
    string RegistrationNumber,
    string? Website,
    string? LinkedIn,
    string? LocationCoordinates,
    string? CompanyLogo,
    string CompanyLocation,
    DateTime DateJoined,
    string VerificationStatus,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateClientRequest(
    string CompanyName,
    string CeoFirstName,
    string CeoLastName,
    string? JobTitle,
    string CompanyEmail,
    string CompanyPhone,
    string CompanyAddress,
    string PostalCode,
    string RegistrationNumber,
    string? Website,
    string? LinkedIn,
    string? LocationCoordinates,
    string? CompanyLogo,
    string CompanyLocation,
    VerificationStatus VerificationStatus = VerificationStatus.Pending
);

public record UpdateClientRequest(
    string? CompanyName,
    string? CeoFirstName,
    string? CeoLastName,
    string? JobTitle,
    string? CompanyEmail,
    string? CompanyPhone,
    string? CompanyAddress,
    string? PostalCode,
    string? RegistrationNumber,
    string? Website,
    string? LinkedIn,
    string? LocationCoordinates,
    string? CompanyLogo,
    string? CompanyLocation
);

public enum VerificationStatus
{
    Pending,
    Verified,
    Rejected,
    Suspended
}