using Test_Consimple.Models.PurchaseItemModels;

namespace Test_Consimple.Models.PurchaseModels;

public class PurchaseResponse
{
    public Guid Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal TotalAmount { get; set; }
    public Guid ClientId { get; set; }
    public List<PurchaseItemResponse> Products { get; set; } = new();
}