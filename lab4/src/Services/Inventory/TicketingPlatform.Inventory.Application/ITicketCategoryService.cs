namespace TicketingPlatform.Inventory.Application;

public interface ITicketCategoryService
{
    Task<TicketCategoryDto> CreateCategoryAsync(string name, int capacity);
    Task<Guid> ReserveTicketsAsync(Guid categoryId, int quantity);
    Task<TicketCategoryDto> GetCategoryAsync(Guid categoryId);
    Task<IReadOnlyList<TicketCategoryDto>> GetAllCategoriesAsync();
    Task DeleteCategoryAsync(Guid categoryId);
}
