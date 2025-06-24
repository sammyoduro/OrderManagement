using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using OrderManagement.Data.Context;
using OrderManagement.Interface;

// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);

// Register the database context as a transient service
builder.Services.AddTransient<OrdersDbContext>();

// Register service and repository dependencies for dependency injection
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

// Add support for API controllers and configure JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as strings in JSON
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Register and configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Order Management System",
        Description = "API Feature Implementation for Order Management System",
        Contact = new OpenApiContact
        {
            Name = "Samuel Oduro",
            Email = "samueloduroonline@gmail.com",
        }
    });

    // Enable annotations like [SwaggerOperation] for better documentation
    options.EnableAnnotations(); 
});

// Build the application
var app = builder.Build();

// Ensure the database is created at startup (for development or testing environments)
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
context.Database.EnsureCreated();

// Enable Swagger UI in the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management API v1");
    });
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Map controller routes (attribute routing)
app.MapControllers();

// Run the web application
app.Run();

// Expose Program class for test projects (e.g., integration tests)
public partial class Program { }
