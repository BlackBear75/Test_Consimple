using Microsoft.EntityFrameworkCore;
using Test_Consimple.Entity.Client;
using Test_Consimple.Entity.Product;
using Test_Consimple.Entity.Purchase;
using Test_Consimple.Entity.PurchaseItem;

namespace Test_Consimple.Configuration;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
       
       DbSet<Client> Clients { get; set; }
       DbSet<Product> Products { get; set; }
       DbSet<Purchase> Purchases { get; set; }
       DbSet<PurchaseItem> PurchaseItems { get; set; }
       
       
}