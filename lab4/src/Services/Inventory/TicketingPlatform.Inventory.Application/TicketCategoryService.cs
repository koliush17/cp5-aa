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

    public async Task<TicketCategoryDto> CreateCategoryAsync(string name, int capacity)
    {
        var category = new TicketCategory(name, capacity);
        await _repository.SaveAsync(category);
        return ToDto(category);
    }

    public async Task<Guid> ReserveTicketsAsync(Guid categoryId, int quantity)
    {
        var category = await GetExistingAsync(categoryId);
        var reservationId = category.Reserve(quantity);
        await _repository.SaveAsync(category);
        return reservationId;
    }

    public async Task<TicketCategoryDto> GetCategoryAsync(Guid categoryId)
    {
        return ToDto(await GetExistingAsync(categoryId));
    }

    public async Task<IReadOnlyList<TicketCategoryDto>> GetAllCategoriesAsync()
    {
        return (await _repository.GetAllAsync()).Select(ToDto).ToList();
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        await _repository.DeleteAsync(await GetExistingAsync(categoryId));
    }

    private async Task<TicketCategory> GetExistingAsync(Guid categoryId)
    {
        return await _repository.GetByIdAsync(categoryId) ?? throw new TicketCategoryNotFoundException(categoryId);
    }

    private static TicketCategoryDto ToDto(TicketCategory category)
    {
        return new TicketCategoryDto(category.Id, category.Name, category.Capacity, category.Reserved, category.Available);
    }
}
