using Test_Consimple.Base;
using Test_Consimple.Base.Repository;
using Test_Consimple.Configuration;
using Test_Consimple.Entity.Client.Repository;

namespace Test_Consimple.Entity.Product.Repository;

public class ProductRepository<TDocument> : BaseRepository<TDocument>, IProductRepository<TDocument> where TDocument : Document
{
    public ProductRepository(AppDbContext  databaseConfiguration) : base(databaseConfiguration)
    {
    }
    
}