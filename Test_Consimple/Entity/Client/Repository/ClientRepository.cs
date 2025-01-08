using Test_Consimple.Base;
using Test_Consimple.Base.Repository;
using Test_Consimple.Configuration;

namespace Test_Consimple.Entity.Client.Repository;

public class ClientRepository<TDocument> : BaseRepository<TDocument>, IClientRepository<TDocument> where TDocument : Document
{
    public ClientRepository(AppDbContext  databaseConfiguration) : base(databaseConfiguration)
    {
    }
    
}