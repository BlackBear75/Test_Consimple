using Test_Consimple.Entity.Product.Repository;
using Test_Consimple.Entity.Purchase;
using Test_Consimple.Entity.PurchaseItem;
using Test_Consimple.Models.PurchaseItemModels;
using Test_Consimple.Models.PurchaseModels;

namespace Test_Consimple.Service;

public interface  IPurchaseService
{
    Task<IEnumerable<PurchaseResponse>> GetAllAsync();
    Task<PurchaseResponse> GetByIdAsync(Guid id);
    Task CreateAsync(CreatePurchaseRequest request);
    Task UpdateAsync(Guid id, UpdatePurchaseRequest request);
    Task DeleteAsync(Guid id);
}

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository<Purchase> _purchaseRepository;

    public PurchaseService(IPurchaseRepository<Purchase> purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<PurchaseResponse>> GetAllAsync()
    {
        var purchases = await _purchaseRepository.GetAllAsync();
        return purchases.Select(p => new PurchaseResponse
        {
            Id = p.Id,
            PurchaseDate = p.PurchaseDate,
            TotalAmount = p.TotalAmount,
            ClientId = p.ClientId,
            Products = p.PurchaseItems.Select(pi => new PurchaseItemResponse
            {
                ProductId = pi.ProductId,
                Quantity = pi.Quantity,
                TotalPrice = pi.TotalPrice
            }).ToList()
        });
    }

    public async Task<PurchaseResponse> GetByIdAsync(Guid id)
    {
        var purchase = await _purchaseRepository.FindByIdAsync(id);
        if (purchase == null)
            throw new KeyNotFoundException("Purchase not found.");

        return new PurchaseResponse
        {
            Id = purchase.Id,
            PurchaseDate = purchase.PurchaseDate,
            TotalAmount = purchase.TotalAmount,
            ClientId = purchase.ClientId,
            Products = purchase.PurchaseItems.Select(pi => new PurchaseItemResponse
            {
                ProductId = pi.ProductId,
                Quantity = pi.Quantity,
                TotalPrice = pi.TotalPrice
            }).ToList()
        };
    }

    public async Task CreateAsync(CreatePurchaseRequest request)
    {
        var purchase = new Purchase
        {
            PurchaseDate = request.PurchaseDate,
            TotalAmount = request.TotalAmount,
            ClientId = request.ClientId,
            PurchaseItems = request.Products.Select(p => new PurchaseItem
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                TotalPrice = p.TotalPrice
            }).ToList()
        };

        await _purchaseRepository.InsertOneAsync(purchase);
    }

    public async Task UpdateAsync(Guid id, UpdatePurchaseRequest request)
    {
        var purchase = await _purchaseRepository.FindByIdAsync(id);
        if (purchase == null)
            throw new KeyNotFoundException("Purchase not found.");

        purchase.PurchaseDate = request.PurchaseDate;
        purchase.TotalAmount = request.TotalAmount;
        purchase.ClientId = request.ClientId;

        purchase.PurchaseItems = request.Products.Select(p => new PurchaseItem
        {
            ProductId = p.ProductId,
            Quantity = p.Quantity,
            TotalPrice = p.TotalPrice
        }).ToList();

        await _purchaseRepository.UpdateOneAsync(purchase);
    }

    public async Task DeleteAsync(Guid id)
    {
        var purchase = await _purchaseRepository.FindByIdAsync(id);
        if (purchase == null)
            throw new KeyNotFoundException("Purchase not found.");

        await _purchaseRepository.DeleteOneAsync(id);
    }
}
