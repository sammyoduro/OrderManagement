using Microsoft.EntityFrameworkCore;
using OrderManagement.Data.Context;
using OrderManagement.Data.Entity;
using OrderManagement.Models;

namespace OrderManagement.Interface;

// Implements the IOrderService interface to provide business logic for order operations
public class OrderService : IOrderService
{
    private readonly OrdersDbContext _context;
    private readonly IDiscountService _discountService;

    // Constructor injection of the database context and discount service
    public OrderService(OrdersDbContext context, IDiscountService discountService)
    {
        _context = context;
        _discountService = discountService;
    }

    // Retrieves all orders from the database asynchronously
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.orders.ToListAsync();
    }

    // Retrieves a single order by its ID
    public async Task<Order> GetOrderAsync(Guid orderId)
    {
        return await _context.orders.FindAsync(orderId);
    }

    // Creates a new order, applies discount, saves to the database, and returns the created order
    public async Task<Order> CreateOrderAsync(OrderDto order)
    {
        // Calculate the discount using the discount service
        decimal discountApplied = _discountService.CalculateDiscount(order);
        order.TotalAmount -= discountApplied;

        // Map the DTO to a new Order entity
        var neworder = new Order()
        {
            Id = Guid.NewGuid(),
            CustomerId = order.CustomerId,
            CustomerCategory = order.CustomerCategory,
            DiscountApplied = discountApplied,
            TotalAmount = order.TotalAmount,
            Status = OrderStatus.Pending,
        };

        // Add and persist the new order
        _context.orders.Add(neworder);
        await _context.SaveChangesAsync();
        
        return neworder;
    }

    // Updates the status of an existing order; sets UpdatedAt only if status is 'Received'
    public async Task<Order?> UpdateStatusAsync(Guid id, OrderStatus newStatus)
    {
        var order = await _context.orders.FindAsync(id);
        if (order == null) return null;

        // Record the time when an order is marked as 'Received'
        if (newStatus == OrderStatus.Received)
            order.UpdatedAt = DateTime.UtcNow;

        order.Status = newStatus;
        await _context.SaveChangesAsync();
        return order;
    }

    // Calculates analytics: average order value and average fulfillment time in minutes
    public async Task<(double avgValue, double avgFulfillTime)> GetAnalyticsAsync()
    {
        // Select only fulfilled orders (i.e., orders with UpdatedAt set)
        var fulfilled = await _context.orders
            .Where(o => o.UpdatedAt != null)
            .ToListAsync();

        // Average order value
        double avgValue = _context.orders.Any() 
            ? _context.orders.Average(o => (double)o.TotalAmount) 
            : 0;

        // Average time between creation and fulfillment
        double avgTime = fulfilled.Any()
            ? fulfilled.Average(o => (o.UpdatedAt!.Value - o.CreatedAt).TotalMinutes)
            : 0;

        return (avgValue, avgTime);
    }
}
