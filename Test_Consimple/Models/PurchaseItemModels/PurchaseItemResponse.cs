namespace Test_Consimple.Models.PurchaseItemModels;

public class PurchaseItemResponse
{
    public Guid Id { get; set; } 
    public Guid PurchaseId { get; set; } 
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } 
    public decimal TotalPrice { get; set; } 
}