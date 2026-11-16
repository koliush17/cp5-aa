using TicketingPlatform.Inventory.Application;
using TicketingPlatform.Inventory.Domain.Exceptions;
using TicketingPlatform.Inventory.Infrastructure;
using TicketingPlatform.Inventory.Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITicketCategoryService, TicketCategoryService>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IInventoryDataSeeder>();
    await seeder.SeedAsync();
}

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
