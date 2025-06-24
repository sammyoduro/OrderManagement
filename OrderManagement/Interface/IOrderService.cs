using OrderManagement.Data.Entity;
using OrderManagement.Models;

namespace OrderManagement.Interface;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderAsync(Guid id);
    Task<Order> CreateOrderAsync(OrderDto order);
    Task<Order?> UpdateStatusAsync(Guid id, OrderStatus newStatus);
    Task<(double avgValue, double avgFulfillTime)> GetAnalyticsAsync();
}