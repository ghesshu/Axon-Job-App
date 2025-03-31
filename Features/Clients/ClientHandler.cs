using System.Threading;
using System.Threading.Tasks;
using Axon_Job_App.Common;
using Axon_Job_App.Data;

namespace Axon_Job_App.Features.Clients;

public class ClientHandler
{
    public Task<CallResult<ClientResponse>> Handle(ClientMutation.CreateClient command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult<ClientResponse>> Handle(ClientMutation.UpdateClient command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(ClientMutation.DeleteClient command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();

    public Task<CallResult> Handle(ClientMutation.UpdateClientVerificationStatus command, DataContext db, CancellationToken ct) 
        => throw new NotImplementedException();
}