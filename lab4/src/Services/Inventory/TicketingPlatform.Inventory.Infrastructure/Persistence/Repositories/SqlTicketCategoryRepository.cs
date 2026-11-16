using Microsoft.EntityFrameworkCore;
using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Infrastructure.Persistence.Repositories;

internal sealed class SqlTicketCategoryRepository : ITicketCategoryRepository
{
    private readonly InventoryDbContext _dbContext;

    public SqlTicketCategoryRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketCategory?> GetByIdAsync(Guid id)
    {
        return await _dbContext.TicketCategories
            .Include(c => c.Reservations)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IReadOnlyList<TicketCategory>> GetAllAsync()
    {
        return await _dbContext.TicketCategories
            .AsNoTracking()
            .Include(c => c.Reservations)
            .ToListAsync();
    }

    public async Task SaveAsync(TicketCategory category)
    {
        if (_dbContext.Entry(category).State == EntityState.Detached)
        {
            await _dbContext.TicketCategories.AddAsync(category);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TicketCategory category)
    {
        _dbContext.TicketCategories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
}
