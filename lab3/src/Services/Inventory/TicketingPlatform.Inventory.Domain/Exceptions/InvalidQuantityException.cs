namespace TicketingPlatform.Inventory.Domain.Exceptions;

public class InvalidQuantityException : DomainException
{
    public int Quantity { get; }

    public InvalidQuantityException(int quantity)
        : base("invalid_quantity", $"Quantity must be greater than zero, but was {quantity}.")
    {
        Quantity = quantity;
    }
}
