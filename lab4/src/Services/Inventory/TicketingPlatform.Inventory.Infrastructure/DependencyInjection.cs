using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Infrastructure.Persistence;
using TicketingPlatform.Inventory.Infrastructure.Persistence.Repositories;
using TicketingPlatform.Inventory.Infrastructure.Persistence.Seeders;

namespace TicketingPlatform.Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
        {
            if (configuration.GetValue<bool>("Persistence:UseInMemoryDatabase"))
                options.UseInMemoryDatabase("InventoryDb");
            else
                options.UseSqlServer(configuration.GetConnectionString("InventoryDb"));
        });

        services.AddScoped<ITicketCategoryRepository, SqlTicketCategoryRepository>();
        services.AddScoped<IInventoryDataSeeder, InventoryDataSeeder>();

        return services;
    }
}
