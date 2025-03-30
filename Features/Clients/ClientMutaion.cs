using axon_final_api.Common;
using Cai;

namespace axon_final_api.Features.Clients;

public class ClientMutation
{
    [Mutation<CallResult<ClientResponse>>]
    public record CreateClient(CreateClientRequest Input);

    [Mutation<CallResult<ClientResponse>>]
    public record UpdateClient(long Id, UpdateClientRequest Input);

    [Mutation<CallResult>]
    public record DeleteClient(long Id);

    [Mutation<CallResult>]
    public record UpdateClientVerificationStatus(long Id, string Status);
}