namespace Test_Consimple.Models.ProductModels;

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
}