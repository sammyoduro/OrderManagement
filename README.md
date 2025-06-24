# Order Management API

An ASP.NET Core 8 Web API for managing customer orders. This project demonstrates clean REST API design, Swagger/OpenAPI documentation, unit/integration testing with xUnit, and basic performance optimizations using Entity Framework Core.

---

## Features

- CRUD operations for managing customer orders
- Discount logic based on customer category and order history
- Analytics endpoint for average order value and fulfillment time
- Swagger UI with annotations for API documentation
- Unit tests (discount logic)
- Integration tests (API endpoints)

---

## Technologies Used

- .NET 8
- Entity Framework Core
- Swashbuckle.AspNetCore (Swagger/OpenAPI)
- xUnit for testing
- Moq for mocking in unit tests
- SqlLite

---

## Testing

### Unit Tests (`DiscountServiceTests.cs`)

Tests verify discount rules:

- Prestige customers with ≥3 previous orders get **20%**
- Prestige customers with <3 previous orders get **15%**
- Regular customers with high average spend get **10%**
- Others get 0%

Example:

```csharp
[Fact]
public void PrestigeCustomer_With3PreviousOrders_Gets20Percent()
{
    var mockRepo = new Mock<IOrderRepository>();
    mockRepo.Setup(r => r.GetOrdersByCustomerId("CUST01")).Returns(new List<Order>
    {
        new Order { TotalAmount = 100 },
        new Order { TotalAmount = 200 },
        new Order { TotalAmount = 300 }
    });

    var service = new DiscountService(mockRepo.Object);
    var order = new OrderDto
    {
        CustomerId = "CUST01",
        CustomerCategory = Category.Prestige,
        TotalAmount = 1000
    };

    var discount = service.CalculateDiscount(order);

    Assert.Equal(200, discount);
}
```

---

### Integration Tests (`OrdersApiTests.cs`)

Tests the actual API endpoints using `WebApplicationFactory<Program>`.

```csharp
[Fact]
public async Task CreateOrder_ReturnsCreated()
{
    var order = new Order
    {
        CustomerId = "CUST01",
        CustomerCategory = Category.Prestige,
        TotalAmount = 1000
    };

    var response = await _client.PostAsJsonAsync("/api/orders/New-Order", order);

    Assert.True(response.IsSuccessStatusCode);
}
```

### Run All Tests

```bash
dotnet test
```

---

## API Endpoints

Base route: `/api/orders`

| Method | Endpoint                      | Description                                  |
| ------ | ----------------------------- | -------------------------------------------- |
| GET    | `/All-Orders`                 | Retrieves a list of all orders               |
| POST   | `/New-Order`                  | Creates a new order                          |
| DELETE | `/Delete-Order/{id}`          | Delete an order                              |
| GET    | `/Get-Order/{id}`             | Retrieves a specific order by ID             |
| PATCH  | `/Update-Order/{id}/{status}` | Updates the order status via query parameter |
| GET    | `/analytics`                  | Returns avg order value and fulfillment time |

---

## Swagger & OpenAPI

Swagger is enabled using Swashbuckle.Annotations.

- Endpoint: [`/swagger`](http://localhost:{port}/swagger)
- JSON: [`/swagger/v1/swagger.json`](http://localhost:{port}/swagger/v1/swagger.json)

### Setup in `Program.cs`:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order Management API",
        Version = "v1",
        Description = "API for managing customer orders",
        Contact = new OpenApiContact
        {
            Name = "Samuel Oduro",
            Email = "samueloduroonline@gmail.com"
        }
    });
    options.EnableAnnotations();
});
```

```csharp
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management API v1");
    c.RoutePrefix = "swagger";
});
```

## Assumptions

- Order ID is a `Guid`
- Discounts depend on order history and customer category
- `OrderDto` is used for incoming order data
- Enum values are serialized as strings (e.g. `"Prestige"`)
- No authentication/authorization is implemented
- SQLite DB used by default for test/demo

---

## Project Structure

```
OrderManagement/
├── Controllers/
│   └── OrdersController.cs
├── Data/
│   └── Context/OrdersDbContext.cs
│   └── Entity/Order.cs
├── Interface/
│   └── IOrderService.cs
│   └── IOrderRepository.cs
│   └── IDiscountService.cs
├── Models/
│   └── OrderDto.cs
├── Services/
│   └── OrderService.cs
│   └── DiscountService.cs
├── Tests/
│   └── DiscountServiceTests.cs
│   └── OrdersApiTests.cs
├── Program.cs
└── README.md
```

---

## 🧑‍💻 Getting Started

1. Clone the repo

```bash
git clone https://github.com/sammyoduro/OrderManagement.git
cd OrderManagement
```

2. Run the project

```bash
dotnet run
```

3. Browse the Swagger UI

```
http://localhost:5155/swagger
```

---

## Author

Samuel Oduro
Email: samueloduroonline@gmail.com
