namespace Test_Consimple.Models.PurchaseModels;

public class RecentBuyerResponse
{
    public Guid ClientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime LastPurchaseDate { get; set; }
}