namespace TicketingPlatform.Inventory.Application;

public record TicketCategoryDto(Guid Id, string Name, int Capacity, int Reserved, int Available);
