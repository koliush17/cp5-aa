using Microsoft.AspNetCore.Mvc;
using TicketingPlatform.Inventory.Application;

namespace TicketingPlatform.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/ticket-categories")]
public class TicketCategoriesController : ControllerBase
{
    private readonly ITicketCategoryService _service;

    public TicketCategoriesController(ITicketCategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create(CreateCategoryRequest request)
    {
        var category = _service.CreateCategory(request.Name, request.Capacity);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        return Ok(_service.GetCategory(id));
    }

    [HttpPost("{id}/reservations")]
    public IActionResult Reserve(Guid id, ReserveRequest request)
    {
        var reservationId = _service.ReserveTickets(id, request.Quantity);
        return Created($"/api/ticket-categories/{id}/reservations/{reservationId}", new { reservationId });
    }
}

public record CreateCategoryRequest(string Name, int Capacity);
public record ReserveRequest(int Quantity);
