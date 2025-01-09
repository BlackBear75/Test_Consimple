using Microsoft.EntityFrameworkCore;
using Test_Consimple.Entity.Client;
using Test_Consimple.Entity.Client.Repository;
using Test_Consimple.Entity.Product.Repository;
using Test_Consimple.Entity.Purchase;
using Test_Consimple.Entity.PurchaseItem;
using Test_Consimple.Entity.PurchaseItem.Repository;
using Test_Consimple.Models.ClientModels;

namespace Test_Consimple.Service;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> GetAllAsync();
    Task<ClientResponse> GetByIdAsync(Guid id);
    Task CreateAsync(CreateClientRequest request);
    Task UpdateAsync(Guid id, UpdateClientRequest request);
    Task DeleteAsync(Guid id);

    Task<IEnumerable<ClientResponse>> GetBirthdaysAsync(int month, int day);
}


public class ClientService : IClientService
{
    private readonly IClientRepository<Client> _clientRepository;
    
    private readonly IPurchaseRepository<Purchase> _purchaseRepository;
    
    private readonly IPurchaseItemRepository<PurchaseItem> _purchaseItemRepository;

    public ClientService(IClientRepository<Client> clientRepository, IPurchaseRepository<Purchase> purchaseRepository, IPurchaseItemRepository<PurchaseItem> purchaseItemRepository)
    {
        _purchaseRepository = purchaseRepository;
        _purchaseItemRepository = purchaseItemRepository;
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<ClientResponse>> GetBirthdaysAsync(int month, int day)
    {
        var clients = await _clientRepository.FilterByAsync(c => 
            c.DateOfBirth.Month == month && c.DateOfBirth.Day == day);

        return clients.Select(c => new ClientResponse
        {
            Id = c.Id,
            FullName = c.FullName
        });
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

        var purchases = await _purchaseRepository
            .Query()
            .Include(p => p.PurchaseItems) 
            .Where(p => p.ClientId == id)
            .ToListAsync();

        foreach (var purchase in purchases)
        {
            foreach (var purchaseItem in purchase.PurchaseItems)
            {
                await _purchaseItemRepository.DeleteOneAsync(purchaseItem.Id);
            }

            await _purchaseRepository.DeleteOneAsync(purchase.Id);
        }

        await _clientRepository.DeleteOneAsync(id);
    }

}
