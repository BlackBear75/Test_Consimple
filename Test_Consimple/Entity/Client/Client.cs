using Test_Consimple.Base;

namespace Test_Consimple.Entity.Client;

public class Client : Document
{
    public string FullName { get; set; } = string.Empty; 
    public DateTime DateOfBirth { get; set; } 
    public DateTime RegistrationDate { get; set; } 

    public ICollection<Purchase.Purchase> Purchases { get; set; } = new List<Purchase.Purchase>();
}