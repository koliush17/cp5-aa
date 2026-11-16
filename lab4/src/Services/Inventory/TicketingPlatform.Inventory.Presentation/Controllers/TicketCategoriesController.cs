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
    public async Task<IActionResult> Create(CreateCategoryRequest request)
    {
        var category = await _service.CreateCategoryAsync(request.Name, request.Capacity);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllCategoriesAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await _service.GetCategoryAsync(id));

    [HttpPost("{id}/reservations")]
    public async Task<IActionResult> Reserve(Guid id, ReserveRequest request)
    {
        var reservationId = await _service.ReserveTicketsAsync(id, request.Quantity);
        return Created($"/api/ticket-categories/{id}/reservations/{reservationId}", new { reservationId });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteCategoryAsync(id);
        return NoContent();
    }
}

public record CreateCategoryRequest(string Name, int Capacity);
public record ReserveRequest(int Quantity);
