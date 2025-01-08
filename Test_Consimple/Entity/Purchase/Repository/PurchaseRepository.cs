using Test_Consimple.Base;
using Test_Consimple.Base.Repository;
using Test_Consimple.Configuration;
using Test_Consimple.Entity.Client.Repository;
using Test_Consimple.Entity.Product.Repository;

namespace Test_Consimple.Entity.Purchase.Repository;

public class PurchaseRepository<TDocument> : BaseRepository<TDocument>, IPurchaseRepository<TDocument> where TDocument : Document
{
    public PurchaseRepository(AppDbContext  databaseConfiguration) : base(databaseConfiguration)
    {
    }
    
}