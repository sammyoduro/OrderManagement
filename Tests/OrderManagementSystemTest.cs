using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderManagement.Data.Entity;
using OrderManagement.Models;

namespace Tests;

public class OrderManagementSystemTest
{
    public class OrdersApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreated()
        {
            // Arrange
            var order = new OrderDto
            {
                CustomerId = "CUST03",
                CustomerCategory = Category.Prestige,
                TotalAmount = 1000
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Orders/New-Order", order);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.True(response.IsSuccessStatusCode, $"Status Code: {response.StatusCode}, Content: {content}");
        }
    }
}