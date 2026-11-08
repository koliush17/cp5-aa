using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Presentation;

public class InMemoryTicketCategoryRepository : ITicketCategoryRepository
{
    private readonly Dictionary<Guid, TicketCategory> _categories = new();

    public TicketCategory? GetById(Guid id)
    {
        return _categories.GetValueOrDefault(id);
    }

    public void Save(TicketCategory category)
    {
        _categories[category.Id] = category;
    }
}
