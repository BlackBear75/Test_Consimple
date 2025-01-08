using Test_Consimple.Base;
using Test_Consimple.Base.Repository;

namespace Test_Consimple.Entity.Product.Repository;

public interface IPurchaseRepository<TDocument> : IBaseRepository<TDocument> where TDocument : Document
{
   
}