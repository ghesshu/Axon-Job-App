using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Axon_Job_App.Data;
using Axon_Job_App.Features.Jobs;
using Axon_Job_App.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Axon_Job_App.Data;

public partial class DataContext
{
    public DbSet<Candidate> Candidates { get; set; } = null!;
}

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


    public class TypeConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.HasMany(c => c.Assignments)
            .WithOne(a => a.Candidate)
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

