using Microsoft.EntityFrameworkCore;
using Test_Consimple.Base.Repository;
using Test_Consimple.Entity.Client.Repository;
using Test_Consimple.Entity.Product.Repository;
using Test_Consimple.Entity.Purchase.Repository;
using Test_Consimple.Entity.PurchaseItem.Repository;
using Test_Consimple.Service;

namespace Test_Consimple.Configuration;

public static class DependencyStartup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        AddDbContext(builder.Services, builder.Configuration);
        AddRepositories(builder.Services);
        AddServices(builder.Services);
        AddInfrastructure(builder.Services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        services.AddScoped(typeof(IClientRepository<>), typeof(ClientRepository<>));
        services.AddScoped(typeof(IProductRepository<>), typeof(ProductRepository<>));
        services.AddScoped(typeof(IPurchaseRepository<>), typeof(PurchaseRepository<>));
        services.AddScoped(typeof(IPurchaseItemRepository<>), typeof(PurchaseItemRepository<>));
     
    }
    
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IPurchaseItemService, PurchaseItemService>();
    
    }


    private static void AddInfrastructure(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }
}
