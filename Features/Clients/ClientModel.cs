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
    public DbSet<Client> Clients { get; set; } = null!;
}

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

    public string? LogoBase64 { get; set; }
    public string? LogoMimeType { get; set; }

    public void SetLogoData(string base64, string? mimeType)
    {
        LogoBase64 = base64;
        LogoMimeType = mimeType;
    }

    public class TypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasMany(c => c.Jobs)
            .WithOne(j => j.Client)
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict if you prefer
        }
    }
}