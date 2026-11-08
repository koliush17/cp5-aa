namespace TicketingPlatform.Inventory.Domain;

public class Reservation
{
    public Guid Id { get; }
    public int Quantity { get; }

    internal Reservation(int quantity)
    {
        Id = Guid.NewGuid();
        Quantity = quantity;
    }
}
