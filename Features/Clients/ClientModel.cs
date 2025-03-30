using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using axon_final_api.Features.Jobs;
using axon_final_api.Helpers;

namespace axon_final_api.Features.Clients;

public class Client : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string CompanyName { get; set; } = null!;

    public string? CompanyImage { get; set; }

    [Required]
    public string CompanyLocation { get; set; } = null!;

    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string VerificationStatus { get; set; } = "pending";

    public int NumberOfAttendingCandidates { get; set; } = 0;

     public virtual ICollection<Job> Jobs { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}