namespace TicketingPlatform.Inventory.Domain.Exceptions;

public class InsufficientCapacityException : DomainException
{
    public int Requested { get; }
    public int Available { get; }

    public InsufficientCapacityException(int requested, int available)
        : base("insufficient_capacity", $"Requested {requested} tickets, but only {available} are available.")
    {
        Requested = requested;
        Available = available;
    }
}
