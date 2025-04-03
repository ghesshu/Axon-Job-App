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
    public async Task EnsureAuthenticated(AuthContext authContext)
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
            await EnsureAuthenticated(authContext);


            var existingClient = await db.Clients
                .FirstOrDefaultAsync(c => c.CompanyName == command.Input.CompanyName, cancellationToken);
            
            if (existingClient != null)
                return CallResult<ClientResponse>.error("Client already exists");

            var client = new Client
            {
                CompanyName = command.Input.CompanyName,
                CompanyImage = command.Input.CompanyImage,
                CompanyLocation = command.Input.CompanyLocation,
                VerificationStatus = command.Input.VerificationStatus.ToString()
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<ClientResponse>.ok(new ClientResponse(
                client.Id,
                client.CompanyName,
                client.CompanyImage,
                client.CompanyLocation,
                client.DateJoined,
                client.VerificationStatus,
                client.CreatedAt,
                client.UpdatedAt
            ), "Client created successfully");
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
            await EnsureAuthenticated(authContext);


            var client = await db.Clients.FindAsync(new object?[] { command.Id }, cancellationToken);
            if (client == null)
                return CallResult<ClientResponse>.error("Client not found");

            if (!string.IsNullOrEmpty(command.Input.CompanyName))
                client.CompanyName = command.Input.CompanyName;

            if (!string.IsNullOrEmpty(command.Input.CompanyImage))
                client.CompanyImage = command.Input.CompanyImage;

            if (!string.IsNullOrEmpty(command.Input.CompanyLocation))
                client.CompanyLocation = command.Input.CompanyLocation;

            // if (command.Input.VerificationStatus.HasValue)
            //     client.VerificationStatus = command.Input.VerificationStatus.Value.ToString();

            client.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            return CallResult<ClientResponse>.ok(new ClientResponse(
                client.Id,
                client.CompanyName,
                client.CompanyImage,
                client.CompanyLocation,
                client.DateJoined,
                client.VerificationStatus,
                client.CreatedAt,
                client.UpdatedAt
            ), "Client updated successfully");
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
            await EnsureAuthenticated(authContext);


            var client = await db.Clients
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);
            
            if (client == null)
                return CallResult.error("Client not found");

            // Check if client has any associated jobs
            if (client.Jobs.Count != 0)
                return CallResult.error("Client cannot be deleted because it has associated jobs");

            db.Clients.Remove(client);
            await db.SaveChangesAsync(cancellationToken);

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
            await EnsureAuthenticated(authContext);

            var client = await db.Clients.FindAsync([command.Id], cancellationToken);
            if (client == null)
                return CallResult.error("Client not found");

            

            client.VerificationStatus = command.Status.ToString();
            client.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(cancellationToken);

            return CallResult.ok("Client verification status updated successfully");
        }
        catch (Exception e)
        {
            return CallResult.error(e.Message);
        }
    }

}