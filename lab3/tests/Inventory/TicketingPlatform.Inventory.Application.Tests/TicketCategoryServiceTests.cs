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
    public void CreateCategory_ValidData_SavesCategoryAndReturnsDto()
    {
        var dto = _service.CreateCategory("VIP", 100);

        Assert.Equal("VIP", dto.Name);
        Assert.Equal(100, dto.Capacity);
        Assert.Equal(100, dto.Available);
        _repository.Verify(r => r.Save(It.IsAny<TicketCategory>()), Times.Once);
    }

    [Fact]
    public void ReserveTickets_ExistingCategory_SavesCategoryAndReturnsReservationId()
    {
        var category = new TicketCategory("VIP", 10);
        _repository.Setup(r => r.GetById(category.Id)).Returns(category);

        var reservationId = _service.ReserveTickets(category.Id, 3);

        Assert.Equal(7, category.Available);
        Assert.Contains(category.Reservations, r => r.Id == reservationId);
        _repository.Verify(r => r.Save(category), Times.Once);
    }

    [Fact]
    public void ReserveTickets_UnknownCategory_ThrowsTicketCategoryNotFound()
    {
        var unknownId = Guid.NewGuid();

        var exception = Assert.Throws<TicketCategoryNotFoundException>(() => _service.ReserveTickets(unknownId, 1));

        Assert.Equal(unknownId, exception.CategoryId);
    }

    [Fact]
    public void ReserveTickets_NotEnoughTickets_ThrowsAndDoesNotSave()
    {
        var category = new TicketCategory("VIP", 2);
        _repository.Setup(r => r.GetById(category.Id)).Returns(category);

        Assert.Throws<InsufficientCapacityException>(() => _service.ReserveTickets(category.Id, 5));

        _repository.Verify(r => r.Save(It.IsAny<TicketCategory>()), Times.Never);
    }

    [Fact]
    public void GetCategory_ExistingCategory_ReturnsDtoWithCurrentNumbers()
    {
        var category = new TicketCategory("VIP", 10);
        category.Reserve(4);
        _repository.Setup(r => r.GetById(category.Id)).Returns(category);

        var dto = _service.GetCategory(category.Id);

        Assert.Equal(category.Id, dto.Id);
        Assert.Equal(4, dto.Reserved);
        Assert.Equal(6, dto.Available);
    }

    [Fact]
    public void GetCategory_UnknownCategory_ThrowsTicketCategoryNotFound()
    {
        Assert.Throws<TicketCategoryNotFoundException>(() => _service.GetCategory(Guid.NewGuid()));
    }
}
