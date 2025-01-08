using Test_Consimple.Base;

namespace Test_Consimple.Entity.PurchaseItem;

public class PurchaseItem : Document
{
    public Guid PurchaseId { get; set; } 
    public Purchase.Purchase Purchase { get; set; } = null!;

    public Guid ProductId { get; set; } 
    public Product.Product Product { get; set; } = null!;

    public int Quantity { get; set; } 
    public decimal TotalPrice { get; set; } 
}