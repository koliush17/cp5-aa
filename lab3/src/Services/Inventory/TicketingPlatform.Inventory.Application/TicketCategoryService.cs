using TicketingPlatform.Inventory.Domain;
using TicketingPlatform.Inventory.Domain.Exceptions;

namespace TicketingPlatform.Inventory.Application;

public class TicketCategoryService : ITicketCategoryService
{
    private readonly ITicketCategoryRepository _repository;

    public TicketCategoryService(ITicketCategoryRepository repository)
    {
        _repository = repository;
    }

    public TicketCategoryDto CreateCategory(string name, int capacity)
    {
        var category = new TicketCategory(name, capacity);
        _repository.Save(category);
        return ToDto(category);
    }

    public Guid ReserveTickets(Guid categoryId, int quantity)
    {
        var category = GetExisting(categoryId);
        var reservationId = category.Reserve(quantity);
        _repository.Save(category);
        return reservationId;
    }

    public TicketCategoryDto GetCategory(Guid categoryId)
    {
        return ToDto(GetExisting(categoryId));
    }

    private TicketCategory GetExisting(Guid categoryId)
    {
        return _repository.GetById(categoryId) ?? throw new TicketCategoryNotFoundException(categoryId);
    }

    private static TicketCategoryDto ToDto(TicketCategory category)
    {
        return new TicketCategoryDto(category.Id, category.Name, category.Capacity, category.Reserved, category.Available);
    }
}
