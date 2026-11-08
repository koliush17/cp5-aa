using TicketingPlatform.Inventory.Domain.Exceptions;

namespace TicketingPlatform.Inventory.Domain;

public class TicketCategory
{
    private readonly List<Reservation> _reservations = new();

    public Guid Id { get; }
    public string Name { get; }
    public int Capacity { get; }
    public IReadOnlyCollection<Reservation> Reservations => _reservations;
    public int Reserved => _reservations.Sum(r => r.Quantity);
    public int Available => Capacity - Reserved;

    public TicketCategory(string name, int capacity)
    {
        if (capacity <= 0)
            throw new InvalidQuantityException(capacity);

        Id = Guid.NewGuid();
        Name = name;
        Capacity = capacity;
    }

    public Guid Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException(quantity);
        if (quantity > Available)
            throw new InsufficientCapacityException(quantity, Available);

        var reservation = new Reservation(quantity);
        _reservations.Add(reservation);
        return reservation.Id;
    }
}
