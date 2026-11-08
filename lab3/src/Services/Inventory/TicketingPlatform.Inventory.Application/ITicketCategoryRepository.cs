using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Application;

public interface ITicketCategoryRepository
{
    TicketCategory? GetById(Guid id);
    void Save(TicketCategory category);
}
