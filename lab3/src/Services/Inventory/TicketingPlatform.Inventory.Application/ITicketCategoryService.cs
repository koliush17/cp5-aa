namespace TicketingPlatform.Inventory.Application;

public interface ITicketCategoryService
{
    TicketCategoryDto CreateCategory(string name, int capacity);
    Guid ReserveTickets(Guid categoryId, int quantity);
    TicketCategoryDto GetCategory(Guid categoryId);
}
