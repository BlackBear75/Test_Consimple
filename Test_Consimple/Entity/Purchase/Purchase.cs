using Test_Consimple.Base;

namespace Test_Consimple.Entity.Purchase;

public class Purchase : Document
{
    public DateTime PurchaseDate { get; set; } 
    public decimal TotalAmount { get; set; } 

    public Guid ClientId { get; set; } 

    public ICollection<PurchaseItem.PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem.PurchaseItem>();
}