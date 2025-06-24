using OrderManagement.Data.Context;
using OrderManagement.Data.Entity;

namespace OrderManagement.Interface;

public class OrderRepository:IOrderRepository
{
    private readonly OrdersDbContext _context;

    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }

    public List<Order> GetOrdersByCustomerId(string customerId)
    {
        return _context.orders
            .Where(o => o.CustomerId == customerId)
            .ToList();
    }
}