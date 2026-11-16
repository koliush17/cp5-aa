using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Application;

public interface ITicketCategoryRepository
{
    Task<TicketCategory?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TicketCategory>> GetAllAsync();
    Task SaveAsync(TicketCategory category);
    Task DeleteAsync(TicketCategory category);
}
