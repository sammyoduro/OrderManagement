using Moq;
using OrderManagement.Data.Entity;
using OrderManagement.Interface;
using OrderManagement.Models;

using Xunit;

namespace Tests;

public class DiscountServiceTests
{
    [Fact]
    public void PrestigeCustomer_With3PreviousOrders_Gets20Percent()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrdersByCustomerId("CUST01"))
            .Returns(new List<Order>
            {
                new Order { CustomerId = "CUST01", TotalAmount = 100 },
                new Order { CustomerId = "CUST01", TotalAmount = 200 },
                new Order { CustomerId = "CUST01", TotalAmount = 300 },
            });

        var service = new DiscountService(mockRepo.Object);
        var order = new OrderDto
        {
            CustomerId = "CUST01",
            CustomerCategory = Category.Prestige,
            TotalAmount = 1000
        };

        var discount = service.CalculateDiscount(order);

        Assert.Equal(200, discount); // 20% of 1000
    }
    [Fact]
    public void PrestigeCustomer_WithNoHistory_Gets15Percent()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrdersByCustomerId("CUST01"))
            .Returns(new List<Order>
            {
                new Order { CustomerId = "CUST01", TotalAmount = 100 },
                
            });

        var service = new DiscountService(mockRepo.Object);
        var order = new OrderDto
        {
            CustomerId = "CUST01",
            CustomerCategory = Category.Prestige,
            TotalAmount = 1000
        };

        var discount = service.CalculateDiscount(order);

        Assert.Equal(150, discount); // 15% of 1000
    }
    [Fact]
    public void RegularCustomer_WithHighAvg_Gets10Percent()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrdersByCustomerId("CUST01")).Returns(new List<Order>
        {
            new Order { CustomerId = "CUST01", TotalAmount = 600 },
            new Order { CustomerId = "CUST01", TotalAmount = 700 }
        });

        var service = new DiscountService(mockRepo.Object);
        var order = new OrderDto
        {
            CustomerId = "CUST01",
            CustomerCategory = Category.Regular,
            TotalAmount = 700
        };

        var discount = service.CalculateDiscount(order);

        Assert.Equal(70, discount); // 10% of 700
    }
    [Fact]
    public void RegularCustomer_WithLowAvg_GetsNoDiscount()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetOrdersByCustomerId("CUST01")).Returns(new List<Order>
        {
            new Order { CustomerId = "CUST01", TotalAmount = 100 },
            new Order { CustomerId = "CUST01", TotalAmount = 200 }
        });

        var service = new DiscountService(mockRepo.Object);
        var order = new OrderDto
        {
            CustomerId = "CUST01",
            CustomerCategory = Category.Regular,
            TotalAmount = 500
        };

        var discount = service.CalculateDiscount(order);

        Assert.Equal(0, discount);
    }
}