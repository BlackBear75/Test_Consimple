namespace Test_Consimple.Models.PurchaseItemModels;

public class UpdatePurchaseItemRequest
{
    public Guid PurchaseId { get; set; } 
    public Guid ProductId { get; set; } 
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; } 
}