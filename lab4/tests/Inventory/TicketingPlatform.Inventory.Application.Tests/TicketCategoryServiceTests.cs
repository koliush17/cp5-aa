using Moq;
using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Domain;
using TicketingPlatform.Inventory.Domain.Exceptions;

namespace TicketingPlatform.Inventory.Application.Tests;

public class TicketCategoryServiceTests
{
    private readonly Mock<ITicketCategoryRepository> _repository = new();
    private readonly TicketCategoryService _service;

    public TicketCategoryServiceTests()
    {
        _service = new TicketCategoryService(_repository.Object);
    }

    [Fact]
    public async Task CreateCategory_ValidData_SavesCategoryAndReturnsDto()
    {
        var dto = await _service.CreateCategoryAsync("VIP", 100);

        Assert.Equal("VIP", dto.Name);
        Assert.Equal(100, dto.Capacity);
        Assert.Equal(100, dto.Available);
        _repository.Verify(r => r.SaveAsync(It.IsAny<TicketCategory>()), Times.Once);
    }

    [Fact]
    public async Task ReserveTickets_ExistingCategory_SavesCategoryAndReturnsReservationId()
    {
        var category = new TicketCategory("VIP", 10);
        _repository.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var reservationId = await _service.ReserveTicketsAsync(category.Id, 3);

        Assert.Equal(7, category.Available);
        Assert.Contains(category.Reservations, r => r.Id == reservationId);
        _repository.Verify(r => r.SaveAsync(category), Times.Once);
    }

    [Fact]
    public async Task ReserveTickets_UnknownCategory_ThrowsTicketCategoryNotFound()
    {
        var unknownId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<TicketCategoryNotFoundException>(() => _service.ReserveTicketsAsync(unknownId, 1));

        Assert.Equal(unknownId, exception.CategoryId);
    }

    [Fact]
    public async Task ReserveTickets_NotEnoughTickets_ThrowsAndDoesNotSave()
    {
        var category = new TicketCategory("VIP", 2);
        _repository.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        await Assert.ThrowsAsync<InsufficientCapacityException>(() => _service.ReserveTicketsAsync(category.Id, 5));

        _repository.Verify(r => r.SaveAsync(It.IsAny<TicketCategory>()), Times.Never);
    }

    [Fact]
    public async Task GetCategory_ExistingCategory_ReturnsDtoWithCurrentNumbers()
    {
        var category = new TicketCategory("VIP", 10);
        category.Reserve(4);
        _repository.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var dto = await _service.GetCategoryAsync(category.Id);

        Assert.Equal(category.Id, dto.Id);
        Assert.Equal(4, dto.Reserved);
        Assert.Equal(6, dto.Available);
    }

    [Fact]
    public async Task GetCategory_UnknownCategory_ThrowsTicketCategoryNotFound()
    {
        await Assert.ThrowsAsync<TicketCategoryNotFoundException>(() => _service.GetCategoryAsync(Guid.NewGuid()));
    }

}
