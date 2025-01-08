using Microsoft.EntityFrameworkCore;
using Test_Consimple.Entity.Client;
using Test_Consimple.Entity.Client.Repository;
using Test_Consimple.Entity.Product;
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
    Task<IEnumerable<RecentBuyerResponse>> GetRecentBuyersAsync(int days);
    Task<IEnumerable<CategoryResponse>> GetPopularCategoriesAsync(Guid clientId);
}

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository<Purchase> _purchaseRepository;
    
    private readonly IClientRepository<Client> _clientRepository;
    
    private readonly IProductRepository<Product> _productRepository;


    public PurchaseService(IPurchaseRepository<Purchase> purchaseRepository, IClientRepository<Client> clientRepository, IProductRepository<Product> productRepository)
    {
        _clientRepository = clientRepository;
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository; 
    }
    
    public async Task<IEnumerable<RecentBuyerResponse>> GetRecentBuyersAsync(int days)
    {
        var dateThreshold = DateTime.UtcNow.AddDays(-days);

        var purchases = await _purchaseRepository.FilterByAsync(p => p.PurchaseDate >= dateThreshold);

        var clientIds = purchases.Select(p => p.ClientId).Distinct();
        var clients = await _clientRepository.FilterByAsync(c => clientIds.Contains(c.Id));

        var recentBuyers = purchases
            .GroupBy(p => p.ClientId)
            .Select(group =>
            {
                var client = clients.FirstOrDefault(c => c.Id == group.Key);
                return new RecentBuyerResponse
                {
                    ClientId = group.Key,
                    FullName = client?.FullName ?? "Unknown",
                    LastPurchaseDate = group.Max(p => p.PurchaseDate)
                };
            });

        return recentBuyers;
    }

    public async Task<IEnumerable<CategoryResponse>> GetPopularCategoriesAsync(Guid clientId)
    {
        var purchases = await _purchaseRepository
            .Query()
            .Include(p => p.PurchaseItems) 
            .ThenInclude(pi => pi.Product)
            .Where(p => p.ClientId == clientId)
            .ToListAsync();

        if (!purchases.Any(p => p.PurchaseItems.Any()))
            return Enumerable.Empty<CategoryResponse>();
        var categories = purchases
            .SelectMany(p => p.PurchaseItems)
            .Where(pi => pi.Product != null)
            .GroupBy(pi => pi.Product.Category)
            .Select(group => new CategoryResponse
            {
                Category = group.Key ?? "Unknown", 
                TotalUnits = group.Sum(pi => pi.Quantity)
            });

        return categories;
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
        decimal totalAmount = 0;

        var purchaseItems = new List<PurchaseItem>();
        foreach (var productRequest in request.Products)
        {
            var product = await _productRepository.FindByIdAsync(productRequest.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productRequest.ProductId} not found.");

            var totalPrice = product.Price * productRequest.Quantity;
            totalAmount += totalPrice;

            purchaseItems.Add(new PurchaseItem
            {
                ProductId = productRequest.ProductId,
                Quantity = productRequest.Quantity,
                TotalPrice = totalPrice
            });
        }

        var purchase = new Purchase
        {
            PurchaseDate = request.PurchaseDate,
            TotalAmount = totalAmount, 
            ClientId = request.ClientId,
            PurchaseItems = purchaseItems
        };

        await _purchaseRepository.InsertOneAsync(purchase);
    }

    public async Task UpdateAsync(Guid id, UpdatePurchaseRequest request)
    {
        var purchase = await _purchaseRepository.FindByIdAsync(id);
        if (purchase == null)
            throw new KeyNotFoundException("Purchase not found.");

        decimal totalAmount = 0;

        var updatedPurchaseItems = new List<PurchaseItem>();
        foreach (var productRequest in request.Products)
        {
            var product = await _productRepository.FindByIdAsync(productRequest.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productRequest.ProductId} not found.");

            var totalPrice = product.Price * productRequest.Quantity;
            totalAmount += totalPrice;

            updatedPurchaseItems.Add(new PurchaseItem
            {
                ProductId = productRequest.ProductId,
                Quantity = productRequest.Quantity,
                TotalPrice = totalPrice
            });
        }

        purchase.PurchaseDate = request.PurchaseDate;
        purchase.TotalAmount = totalAmount; 
        purchase.ClientId = request.ClientId;
        purchase.PurchaseItems = updatedPurchaseItems;

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
