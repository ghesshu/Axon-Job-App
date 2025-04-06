using Axon_Job_App.Features.Users;
using Axon_Job_App.Features.Clients;
using Axon_Job_App.Features.Jobs;
using Axon_Job_App.Features.Candidates;
using Axon_Job_App.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Axon_Job_App.Data;

public class DataSeeder
{
    public static async Task SeedData(DataContext context)
    {
        // Seed Permissions
        var permissions = Permissions.All.Select(p => new Permission
        {
            Name = p,
            Description = $"Permission to {p}"
        }).ToList();

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();

        // Create Roles
        var adminRole = new Role
        {
            Name = "Admin",
            Description = "Administrator role with full access",
            RolePermissions = [.. permissions.Select(p => new RolePermission
            {
                Permission = p
            })]
        };

        // var clientRole = new Role
        // {
        //     Name = "Client",
        //     Description = "Client role with limited access"
        // };

        // var candidateRole = new Role
        // {
        //     Name = "Candidate",
        //     Description = "Candidate role with basic access"
        // };

        await context.Roles.AddRangeAsync(adminRole);
        await context.SaveChangesAsync();

        // Create Admin User
        if (!await context.Users.AnyAsync(u => u.Email == "admin@axon.com"))
        {
            var adminUser = new User
            {
                Email = "admin@axon.com",
                RoleId = adminRole.Id
            };
            adminUser.SetPassword("Admin@123");

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // Seed Candidates
        if (!await context.Set<Candidate>().AnyAsync())
        {
            var candidates = new[]
            {
                new Candidate
                {
                    Name = "John Smith",
                    Email = "john.smith@example.com",
                    Phone = "+1-555-0101",
                    Skills = ["C#", "ASP.NET Core", "React", "SQL"],
                    Experience = "5 years of full-stack development experience",
                    Verified = true
                },
                new Candidate
                {
                    Name = "Sarah Johnson",
                    Email = "sarah.j@example.com",
                    Phone = "+1-555-0102",
                    Skills = ["Python", "Django", "PostgreSQL", "AWS"],
                    Experience = "3 years as a backend developer",
                    Verified = true
                },
                new Candidate
                {
                    Name = "Michael Chen",
                    Email = "m.chen@example.com",
                    Phone = "+1-555-0103",
                    Skills = ["JavaScript", "Vue.js", "Node.js", "MongoDB"],
                    Experience = "4 years of frontend development",
                    Verified = true
                },
                new Candidate
                {
                    Name = "Emma Davis",
                    Email = "emma.d@example.com",
                    Phone = "+1-555-0104",
                    Skills = ["Java", "Spring Boot", "Angular", "Oracle"],
                    Experience = "6 years in enterprise software development",
                    Verified = true
                },
                new Candidate
                {
                    Name = "Alex Turner",
                    Email = "alex.t@example.com",
                    Phone = "+1-555-0105",
                    Skills = ["Ruby", "Rails", "React", "PostgreSQL"],
                    Experience = "2 years of web development",
                    Verified = false
                }
            };

            await context.Set<Candidate>().AddRangeAsync(candidates);
            await context.SaveChangesAsync();
        }

        // Seed Clients
        if (!await context.Set<Client>().AnyAsync())
        {
            var clients = new[]
            {
                new Client
                {
                    CompanyName = "Facebook",
                    CeoFirstName = "Mark",
                    CeoLastName = "Zuckerberg",
                    JobTitle = "CEO",
                    CompanyEmail = "recruitment@facebook.com",
                    CompanyPhone = "+1-650-555-0123",
                    CompanyAddress = "1 Hacker Way",
                    PostalCode = "94025",
                    RegistrationNumber = "FB123456789",
                    Website = "https://facebook.com",
                    LinkedIn = "https://linkedin.com/company/facebook",
                    LocationCoordinates = "37.4848,-122.1483",
                    CompanyLogo = "https://upload.wikimedia.org/wikipedia/commons/5/51/Facebook_f_logo_%282019%29.svg",
                    CompanyLocation = "Menlo Park, CA",
                    VerificationStatus = "verified"
                },
                new Client
                {
                    CompanyName = "Amazon",
                    CeoFirstName = "Andy",
                    CeoLastName = "Jassy",
                    JobTitle = "CEO",
                    CompanyEmail = "careers@amazon.com",
                    CompanyPhone = "+1-206-555-0123",
                    CompanyAddress = "410 Terry Ave N",
                    PostalCode = "98109",
                    RegistrationNumber = "AMZ987654321",
                    Website = "https://amazon.com",
                    LinkedIn = "https://linkedin.com/company/amazon",
                    LocationCoordinates = "47.6205,-122.3493",
                    CompanyLogo = "https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg",
                    CompanyLocation = "Seattle, WA",
                    VerificationStatus = "verified"
                },
                new Client
                {
                    CompanyName = "Tesla",
                    CeoFirstName = "Elon",
                    CeoLastName = "Musk",
                    JobTitle = "CEO",
                    CompanyEmail = "jobs@tesla.com",
                    CompanyPhone = "+1-510-555-0123",
                    CompanyAddress = "3500 Deer Creek Road",
                    PostalCode = "94304",
                    RegistrationNumber = "TSL456789123",
                    Website = "https://tesla.com",
                    LinkedIn = "https://linkedin.com/company/tesla",
                    LocationCoordinates = "37.3948,-122.1503",
                    CompanyLogo = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e8/Tesla_logo.png/480px-Tesla_logo.png",
                    CompanyLocation = "Palo Alto, CA",
                    VerificationStatus = "verified"
                }
            };

            await context.Set<Client>().AddRangeAsync(clients);
            await context.SaveChangesAsync();

            // Seed Jobs for each client
            var jobs = new List<Job>();
            foreach (var client in clients)
            {
                var clientJobs = new[]
                {
                    new Job
                    {
                        ClientId = client.Id,
                        Title = $"Senior Software Engineer at {client.CompanyName}",
                        JobType = JobType.Permanent,  // Changed from FullTime to Permanent
                        Status = JobStatus.Published,  // Changed from Open to Published
                        PaymentType = PaymentType.PerAnnum,  // Changed from Salary to PerAnnum
                        SalaryPerAnnum = 150000M,
                        Duties = ["Lead technical projects", "Mentor junior developers", "Design system architecture", "Code review"],
                        Requirements = ["5+ years experience", "BS in Computer Science", "Strong leadership skills", "Cloud expertise"],
                        JobHours = "40 hours per week",
                        Location = client.CompanyLocation,
                        StartDate = DateTime.UtcNow.AddDays(30),
                        NumberOfRoles = 2,
                        Published = true
                    },
                    new Job
                    {
                        ClientId = client.Id,
                        Title = $"Product Manager at {client.CompanyName}",
                        JobType = JobType.Permanent,  // Changed from FullTime to Permanent
                        Status = JobStatus.Published,  // Changed from Open to Published
                        PaymentType = PaymentType.PerAnnum,  // Changed from Salary to PerAnnum
                        SalaryPerAnnum = 130000M,
                        Duties = ["Product strategy", "Feature prioritization", "Stakeholder management", "Market research"],
                        Requirements = ["3+ years PM experience", "MBA preferred", "Technical background", "Agile expertise"],
                        JobHours = "40 hours per week",
                        Location = client.CompanyLocation,
                        StartDate = DateTime.UtcNow.AddDays(45),
                        NumberOfRoles = 1,
                        Published = true
                    }
                };
                jobs.AddRange(clientJobs);
            }

            await context.Set<Job>().AddRangeAsync(jobs);
            await context.SaveChangesAsync();

            // Create job assignments
            var candidates = await context.Set<Candidate>().ToListAsync();
            var publishedJobs = await context.Set<Job>().Where(j => j.Published).ToListAsync();

            if (candidates.Any() && publishedJobs.Any())
            {
                var assignments = new List<JobAssignment>();
                var random = new Random();

                foreach (var job in publishedJobs.Take(3))
                {
                    var randomCandidates = candidates
                        .OrderBy(x => random.Next())
                        .Take(2)
                        .ToList();

                    foreach (var candidate in randomCandidates)
                    {
                        assignments.Add(new JobAssignment
                        {
                            JobId = job.Id,
                            CandidateId = candidate.Id,
                            Status = AssignmentStatus.Active
                        });
                    }
                }

                await context.Set<JobAssignment>().AddRangeAsync(assignments);
                await context.SaveChangesAsync();
            }
        }
    }
}