namespace TicketingPlatform.Inventory.Domain;

public class Reservation
{
    public Guid Id { get; private set; }
    public int Quantity { get; private set; }

    private Reservation()
    {
    }

    internal Reservation(int quantity)
    {
        Id = Guid.NewGuid();
        Quantity = quantity;
    }
}
