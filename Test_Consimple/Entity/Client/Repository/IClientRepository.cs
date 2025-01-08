using Test_Consimple.Base;
using Test_Consimple.Base.Repository;

namespace Test_Consimple.Entity.Client.Repository;

public interface IClientRepository<TDocument> : IBaseRepository<TDocument> where TDocument : Document
{
   
}