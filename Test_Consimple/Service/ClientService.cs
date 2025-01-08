using Test_Consimple.Entity.Client;
using Test_Consimple.Entity.Client.Repository;
using Test_Consimple.Models.ClientModels;

namespace Test_Consimple.Service;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> GetAllAsync();
    Task<ClientResponse> GetByIdAsync(Guid id);
    Task CreateAsync(CreateClientRequest request);
    Task UpdateAsync(Guid id, UpdateClientRequest request);
    Task DeleteAsync(Guid id);
}


public class ClientService : IClientService
{
    private readonly IClientRepository<Client> _clientRepository;

    public ClientService(IClientRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<ClientResponse>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.Select(c => new ClientResponse
        {
            Id = c.Id,
            FullName = c.FullName,
            DateOfBirth = c.DateOfBirth,
            RegistrationDate = c.RegistrationDate
        });
    }

    public async Task<ClientResponse> GetByIdAsync(Guid id)
    {
        var client = await _clientRepository.FindByIdAsync(id);
        if (client == null)
            throw new KeyNotFoundException("Client not found.");

        return new ClientResponse
        {
            Id = client.Id,
            FullName = client.FullName,
            DateOfBirth = client.DateOfBirth,
            RegistrationDate = client.RegistrationDate
        };
    }

    public async Task CreateAsync(CreateClientRequest request)
    {
        var client = new Client
        {
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            RegistrationDate = DateTime.UtcNow
        };

        await _clientRepository.InsertOneAsync(client);
    }

    public async Task UpdateAsync(Guid id, UpdateClientRequest request)
    {
        var client = await _clientRepository.FindByIdAsync(id);
        if (client == null)
            throw new KeyNotFoundException("Client not found.");

        client.FullName = request.FullName;
        client.DateOfBirth = request.DateOfBirth;

        await _clientRepository.UpdateOneAsync(client);
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _clientRepository.FindByIdAsync(id);
        if (client == null)
            throw new KeyNotFoundException("Client not found.");

        await _clientRepository.DeleteOneAsync(id);
    }
}
