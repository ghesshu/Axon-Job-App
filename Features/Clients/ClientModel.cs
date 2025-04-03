using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Axon_Job_App.Features.Jobs;
using Axon_Job_App.Helpers;

namespace Axon_Job_App.Features.Clients;

public class Client : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string CompanyName { get; set; } = null!;

    [Required]
    public string CeoFirstName { get; set; } = null!;

    [Required]
    public string CeoLastName { get; set; } = null!;

    public string? JobTitle { get; set; }

    [Required]
    [EmailAddress]
    public string CompanyEmail { get; set; } = null!;

    [Required]
    [Phone]
    public string CompanyPhone { get; set; } = null!;

    [Required]
    public string CompanyAddress { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string RegistrationNumber { get; set; } = null!;

    public string? Website { get; set; }

    public string? LinkedIn { get; set; }

    public string? LocationCoordinates { get; set; }

    public string? CompanyLogo { get; set; }

    [Required]
    public string CompanyLocation { get; set; } = null!;

    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string VerificationStatus { get; set; } = "pending";

    public virtual ICollection<Job> Jobs { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}