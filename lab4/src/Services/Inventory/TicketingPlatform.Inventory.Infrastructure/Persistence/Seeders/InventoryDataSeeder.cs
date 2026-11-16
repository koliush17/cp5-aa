using Microsoft.EntityFrameworkCore;
using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Infrastructure.Persistence.Seeders;

internal sealed class InventoryDataSeeder : IInventoryDataSeeder
{
    private readonly InventoryDbContext _dbContext;

    public InventoryDataSeeder(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        if (_dbContext.Database.IsRelational())
            await _dbContext.Database.MigrateAsync();
        else
            await _dbContext.Database.EnsureCreatedAsync();

        if (await _dbContext.TicketCategories.AnyAsync())
            return;

        var vip = new TicketCategory("VIP", 100);
        vip.Reserve(10);
        var backstage = new TicketCategory("Backstage", 2);

        await _dbContext.TicketCategories.AddRangeAsync(vip, backstage);
        await _dbContext.SaveChangesAsync();
    }
}
