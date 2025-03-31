using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Axon_Job_App.Features.Jobs;
using Axon_Job_App.Helpers;

namespace Axon_Job_App.Features.Candidates;

public class Candidate : IHasTimestamps
{
    [Key]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Phone { get; set; } = null!;

    [Column(TypeName = "text[]")]
    public string[] Skills { get; set; } = Array.Empty<string>();

    public string? Experience { get; set; }

    public bool Verified { get; set; } = false;

    public virtual ICollection<JobAssignment> Assignments { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}