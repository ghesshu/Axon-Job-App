using System.Threading;
using System.Threading.Tasks;
using axon_final_api.Common;
using axon_final_api.Data;

namespace axon_final_api.Features.Clients;

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