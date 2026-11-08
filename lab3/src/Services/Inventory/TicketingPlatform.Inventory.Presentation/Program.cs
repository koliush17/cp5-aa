using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Domain.Exceptions;
using TicketingPlatform.Inventory.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITicketCategoryRepository, InMemoryTicketCategoryRepository>();
builder.Services.AddScoped<ITicketCategoryService, TicketCategoryService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (DomainException ex)
    {
        context.Response.StatusCode = ex is TicketCategoryNotFoundException ? 404 : 422;
        await context.Response.WriteAsJsonAsync(new { code = ex.Code, message = ex.Message });
    }
});

app.MapControllers();

app.Run();
