using Test_Consimple.Base;

namespace Test_Consimple.Entity.Product;

public class Product : Document
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
    public string SKU { get; set; } = string.Empty; 
    public decimal Price { get; set; } 
}