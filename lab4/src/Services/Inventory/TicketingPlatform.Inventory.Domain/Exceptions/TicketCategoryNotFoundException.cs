namespace TicketingPlatform.Inventory.Domain.Exceptions;

public class TicketCategoryNotFoundException : DomainException
{
    public Guid CategoryId { get; }

    public TicketCategoryNotFoundException(Guid categoryId)
        : base("ticket_category_not_found", $"Ticket category {categoryId} was not found.")
    {
        CategoryId = categoryId;
    }
}
