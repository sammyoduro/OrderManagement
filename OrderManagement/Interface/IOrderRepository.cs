using OrderManagement.Data.Entity;

namespace OrderManagement.Interface;

public interface IOrderRepository
{
    List<Order> GetOrdersByCustomerId(string customerId);
}