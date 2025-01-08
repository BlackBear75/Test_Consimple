using Test_Consimple.Base;
using Test_Consimple.Base.Repository;

namespace Test_Consimple.Entity.PurchaseItem.Repository;

public interface IPurchaseItemRepository<TDocument> : IBaseRepository<TDocument> where TDocument : Document
{
   
}