using Test_Consimple.Entity.PurchaseItem;
using Test_Consimple.Entity.PurchaseItem.Repository;
using Test_Consimple.Models.PurchaseItemModels;

namespace Test_Consimple.Service;


public interface IPurchaseItemService
{
    Task<IEnumerable<PurchaseItemResponse>> GetAllAsync(Guid purchaseId);
    Task<PurchaseItemResponse> GetByIdAsync(Guid id); 
    Task CreateAsync(Guid purchaseId, CreatePurchaseItemRequest request); 
    Task UpdateAsync(Guid id, UpdatePurchaseItemRequest request); 
    Task DeleteAsync(Guid id); 
}

public class PurchaseItemService : IPurchaseItemService
{
    private readonly IPurchaseItemRepository<PurchaseItem> _purchaseItemRepository;

    public PurchaseItemService(IPurchaseItemRepository<PurchaseItem> purchaseItemRepository)
    {
        _purchaseItemRepository = purchaseItemRepository;
    }

    public async Task<IEnumerable<PurchaseItemResponse>> GetAllAsync(Guid purchaseId)
    {
        var purchaseItems = await _purchaseItemRepository.FilterByAsync(pi => pi.PurchaseId == purchaseId);
        return purchaseItems.Select(pi => new PurchaseItemResponse
        {
            Id = pi.Id,
            PurchaseId = pi.PurchaseId,
            ProductId = pi.ProductId,
            Quantity = pi.Quantity,
            TotalPrice = pi.TotalPrice
        });
    }

    public async Task<PurchaseItemResponse> GetByIdAsync(Guid id)
    {
        var purchaseItem = await _purchaseItemRepository.FindByIdAsync(id);
        if (purchaseItem == null)
            throw new KeyNotFoundException("PurchaseItem not found.");

        return new PurchaseItemResponse
        {
            Id = purchaseItem.Id,
            PurchaseId = purchaseItem.PurchaseId,
            ProductId = purchaseItem.ProductId,
            Quantity = purchaseItem.Quantity,
            TotalPrice = purchaseItem.TotalPrice
        };
    }

    public async Task CreateAsync(Guid purchaseId, CreatePurchaseItemRequest request)
    {
        var purchaseItem = new PurchaseItem
        {
            PurchaseId = purchaseId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            TotalPrice = request.TotalPrice
        };

        await _purchaseItemRepository.InsertOneAsync(purchaseItem);
    }

    public async Task UpdateAsync(Guid id, UpdatePurchaseItemRequest request)
    {
        var purchaseItem = await _purchaseItemRepository.FindByIdAsync(id);
        if (purchaseItem == null)
            throw new KeyNotFoundException("PurchaseItem not found.");

        purchaseItem.PurchaseId = request.PurchaseId;
        purchaseItem.ProductId = request.ProductId;
        purchaseItem.Quantity = request.Quantity;
        purchaseItem.TotalPrice = request.TotalPrice;

        await _purchaseItemRepository.UpdateOneAsync(purchaseItem);
    }

    public async Task DeleteAsync(Guid id)
    {
        var purchaseItem = await _purchaseItemRepository.FindByIdAsync(id);
        if (purchaseItem == null)
            throw new KeyNotFoundException("PurchaseItem not found.");

        await _purchaseItemRepository.DeleteOneAsync(id);
    }
}