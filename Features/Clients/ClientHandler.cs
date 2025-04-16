using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;
using Microsoft.EntityFrameworkCore;
using Axon_Job_App.Common.Extensions;
using Axon_Job_App.Services;

namespace Axon_Job_App.Features.Clients;

public class ClientHandler(AuthContext authContext)
{
        private async Task EnsureAuthenticated()
    {
        if (!await Task.FromResult(authContext.IsAuthenticated()))
        {
            throw new UnauthorizedAccessException("Unauthorized");
        }
    }

    public async Task<CallResult<ClientResponse>> Handle(
        ClientMutation.CreateClient command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureAuthenticated();
            // Use Any() instead of FirstOrDefaultAsync for existence check - better performance
            if (await db.Clients.AnyAsync(c => c.CompanyName == command.Input.CompanyName, cancellationToken))
                return CallResult<ClientResponse>.error("Client already exists");

            var client = new Client
            {
                CompanyName = command.Input.CompanyName,
                CeoFirstName = command.Input.CeoFirstName,
                CeoLastName = command.Input.CeoLastName,
                JobTitle = command.Input.JobTitle,
                CompanyEmail = command.Input.CompanyEmail,
                CompanyPhone = command.Input.CompanyPhone,
                CompanyAddress = command.Input.CompanyAddress,
                PostalCode = command.Input.PostalCode,
                RegistrationNumber = command.Input.RegistrationNumber,
                Website = command.Input.Website,
                LinkedIn = command.Input.LinkedIn,
                LocationCoordinates = command.Input.LocationCoordinates,
                CompanyLogo = command.Input.CompanyLogo,
                CompanyLocation = command.Input.CompanyLocation,
                VerificationStatus = command.Input.VerificationStatus.ToString(),
            };
            
            if (command.Input.LogoImg != null)
            {
                using var ms = new MemoryStream();
                await command.Input.LogoImg.CopyToAsync(ms, cancellationToken);
                client.SetLogoData(Convert.ToBase64String(ms.ToArray()), command.Input.LogoImg.ContentType);
            }

            // Use AddAsync for better async performance
            await db.Clients.AddAsync(client, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<ClientResponse>.ok(MapToResponse(client), "Client created successfully");
        }
        catch (Exception e)
        {
            return CallResult<ClientResponse>.error(e.Message);
        }
    }

    public async Task<CallResult<ClientResponse>> Handle(
        ClientMutation.UpdateClient command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureAuthenticated();
            var client = await db.Clients.FindAsync([command.Id], cancellationToken);
            if (client == null)
                return CallResult<ClientResponse>.error("Client not found");

            // Use pattern matching for cleaner null checks
            UpdateClientProperties(client, command.Input);
            
            if (command.Input.CompanyLogo != null)
            {
                using var ms = new MemoryStream();
                await command.Input.LogoImg.CopyToAsync(ms, cancellationToken);
                client.SetLogoData(Convert.ToBase64String(ms.ToArray()), command.Input.LogoImg.ContentType);
            }

            client.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<ClientResponse>.ok(MapToResponse(client), "Client updated successfully");
        }
        catch (Exception e)
        {
            return CallResult<ClientResponse>.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        ClientMutation.DeleteClient command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureAuthenticated();
            // Use a single query to check both existence and job count
            var clientWithJobs = await db.Clients
                .Select(c => new { c.Id, JobCount = c.Jobs.Count })
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

            if (clientWithJobs == null)
                return CallResult.error("Client not found");

            if (clientWithJobs.JobCount > 0)
                return CallResult.error("Client cannot be deleted because it has associated jobs");

            // Use ExecuteDeleteAsync for better performance
            await db.Clients
                .Where(c => c.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return CallResult.ok("Client deleted successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    public async Task<CallResult> Handle(
        ClientMutation.ClientVStatus command, 
        DataContext db, 
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureAuthenticated();
            // Use ExecuteUpdateAsync for better performance
            var updated = await db.Clients
                .Where(c => c.Id == command.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.VerificationStatus, command.Status.ToString())
                    .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                    cancellationToken);

            if (updated == 0)
                return CallResult.error("Client not found");

            return CallResult.ok("Client verification status updated successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

    // Helper methods for cleaner code
    private static ClientResponse MapToResponse(Client client) => new(
        client.Id,
        client.CompanyName,
        client.CeoFirstName,
        client.CeoLastName,
        client.JobTitle,
        client.CompanyEmail,
        client.CompanyPhone,
        client.CompanyAddress,
        client.PostalCode,
        client.RegistrationNumber,
        client.Website,
        client.LinkedIn,
        client.LocationCoordinates,
        client.CompanyLogo,
        client.CompanyLocation,
        client.DateJoined,
        client.VerificationStatus,
        client.CreatedAt,
        client.UpdatedAt
    );

    private static void UpdateClientProperties(Client client, UpdateClientRequest input)
    {
        if (!string.IsNullOrEmpty(input.CompanyName)) client.CompanyName = input.CompanyName;
        if (!string.IsNullOrEmpty(input.CeoFirstName)) client.CeoFirstName = input.CeoFirstName;
        if (!string.IsNullOrEmpty(input.CeoLastName)) client.CeoLastName = input.CeoLastName;
        if (input.JobTitle != null) client.JobTitle = input.JobTitle;
        if (!string.IsNullOrEmpty(input.CompanyEmail)) client.CompanyEmail = input.CompanyEmail;
        if (!string.IsNullOrEmpty(input.CompanyPhone)) client.CompanyPhone = input.CompanyPhone;
        if (!string.IsNullOrEmpty(input.CompanyAddress)) client.CompanyAddress = input.CompanyAddress;
        if (!string.IsNullOrEmpty(input.PostalCode)) client.PostalCode = input.PostalCode;
        if (!string.IsNullOrEmpty(input.RegistrationNumber)) client.RegistrationNumber = input.RegistrationNumber;
        if (input.Website != null) client.Website = input.Website;
        if (input.LinkedIn != null) client.LinkedIn = input.LinkedIn;
        if (input.LocationCoordinates != null) client.LocationCoordinates = input.LocationCoordinates;
        if (input.CompanyLogo != null) client.CompanyLogo = input.CompanyLogo;
        if (!string.IsNullOrEmpty(input.CompanyLocation)) client.CompanyLocation = input.CompanyLocation;
    }
}