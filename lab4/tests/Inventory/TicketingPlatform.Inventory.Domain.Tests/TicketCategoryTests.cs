using TicketingPlatform.Inventory.Domain;
using TicketingPlatform.Inventory.Domain.Exceptions;

namespace TicketingPlatform.Inventory.Domain.Tests;

public class TicketCategoryTests
{
    [Fact]
    public void Constructor_ValidData_StartsWithAllTicketsAvailable()
    {
        var category = new TicketCategory("VIP", 100);

        Assert.Equal("VIP", category.Name);
        Assert.Equal(100, category.Capacity);
        Assert.Equal(0, category.Reserved);
        Assert.Equal(100, category.Available);
    }

    [Fact]
    public void Reserve_ValidQuantity_ReducesAvailableTickets()
    {
        var category = new TicketCategory("VIP", 100);

        category.Reserve(30);

        Assert.Equal(30, category.Reserved);
        Assert.Equal(70, category.Available);
    }

    [Fact]
    public void Reserve_ExactlyAllAvailable_Succeeds()
    {
        var category = new TicketCategory("VIP", 10);

        category.Reserve(10);

        Assert.Equal(0, category.Available);
    }

    [Fact]
    public void Reserve_MoreThanAvailable_ThrowsInsufficientCapacity()
    {
        var category = new TicketCategory("VIP", 10);
        category.Reserve(8);

        var exception = Assert.Throws<InsufficientCapacityException>(() => category.Reserve(5));

        Assert.Equal(5, exception.Requested);
        Assert.Equal(2, exception.Available);
        Assert.Equal(8, category.Reserved);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Reserve_ZeroOrNegativeQuantity_ThrowsInvalidQuantity(int quantity)
    {
        var category = new TicketCategory("VIP", 10);

        var exception = Assert.Throws<InvalidQuantityException>(() => category.Reserve(quantity));

        Assert.Equal(quantity, exception.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ZeroOrNegativeCapacity_ThrowsInvalidQuantity(int capacity)
    {
        var exception = Assert.Throws<InvalidQuantityException>(() => new TicketCategory("VIP", capacity));

        Assert.Equal(capacity, exception.Quantity);
    }
}
