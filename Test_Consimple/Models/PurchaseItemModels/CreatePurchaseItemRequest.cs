namespace Test_Consimple.Models.PurchaseItemModels;

public class CreatePurchaseItemRequest
{

    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}