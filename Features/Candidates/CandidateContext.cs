using System;
using Axon_Job_App.Features.Candidates;
using Axon_Job_App.Features.Clients;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Data;

public partial class DataContext
{
     public DbSet<Candidate> Candidates { get; set; } = null!;

    private static void ConfigureCandidates(ModelBuilder modelBuilder)
    {
        // Configure Client-Job relationship (One-to-Many)
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Jobs)
            .WithOne(j => j.Client)
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict if you prefer
       
    }
}
