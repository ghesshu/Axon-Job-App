using System;
using Axon_Job_App.Features.Jobs;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Data;

public partial class DataContext
{
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<JobAssignment> JobAssignments { get; set; } = null!;

    private static void ConfigureJobs(ModelBuilder modelBuilder)
    {
        // Configure Job-Candidate many-to-many via JobAssignment
        modelBuilder.Entity<JobAssignment>()
            .HasKey(ja => new { ja.JobId, ja.CandidateId }); // Composite primary key

        modelBuilder.Entity<JobAssignment>()
            .HasOne(ja => ja.Job)
            .WithMany(j => j.Assignments)
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict

        modelBuilder.Entity<JobAssignment>()
            .HasOne(ja => ja.Candidate)
            .WithMany(c => c.Assignments)
            .HasForeignKey(ja => ja.CandidateId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict

        // Enum conversions 
        modelBuilder.Entity<Job>()
            .Property(j => j.JobType)
            .HasConversion<string>();

        modelBuilder.Entity<Job>()
            .Property(j => j.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Job>()
            .Property(j => j.PaymentType)
            .HasConversion<string>();

        modelBuilder.Entity<JobAssignment>()
            .Property(ja => ja.Status)
            .HasConversion<string>();
    }
}
