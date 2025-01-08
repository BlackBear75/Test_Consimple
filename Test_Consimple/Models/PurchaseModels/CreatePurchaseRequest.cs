using Test_Consimple.Models.PurchaseItemModels;

namespace Test_Consimple.Models.PurchaseModels;

public class CreatePurchaseRequest
{
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid ClientId { get; set; }
    public List<CreatePurchaseItemRequest> Products { get; set; } = new();
}