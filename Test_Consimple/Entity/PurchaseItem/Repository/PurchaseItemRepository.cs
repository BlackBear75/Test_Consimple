using Test_Consimple.Base;
using Test_Consimple.Base.Repository;
using Test_Consimple.Configuration;
using Test_Consimple.Entity.Client.Repository;

namespace Test_Consimple.Entity.PurchaseItem.Repository;

public class PurchaseItemRepository<TDocument> : BaseRepository<TDocument>, IPurchaseItemRepository<TDocument> where TDocument : Document
{
    public PurchaseItemRepository(AppDbContext  databaseConfiguration) : base(databaseConfiguration)
    {
    }
    
}