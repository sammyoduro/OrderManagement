using OrderManagement.Data.Context;
using OrderManagement.Data.Entity;
using OrderManagement.Models;

namespace OrderManagement.Interface;

public class DiscountService: IDiscountService
{
    private readonly IOrderRepository _repository;

    public DiscountService(IOrderRepository repository)
    {
        _repository = repository;
    }
    public decimal CalculateDiscount(OrderDto order)
    {
        
        var previousOrders = _repository.GetOrdersByCustomerId(order.CustomerId);
        
        //check if there is an existing order made by same customer
        var CountPreviousOrders = previousOrders.Count; 
        
        //find the 
        var avgPrevious = previousOrders.Any() ? previousOrders.Average(o => o.TotalAmount) : 0;
        
        var segment = order.CustomerCategory;
        decimal discount = 0;
        
        if (order.CustomerCategory == Category.Prestige)
        {
            //Prestige customers are automatically given 15% discount regardless of their order history
            discount += order.TotalAmount * 0.15m;
            
            /*
             Extra discount of 5% is given if prestige customer has 3 or more order history which adds up to 
            their existing 15% making 20% in total
            */
            if (CountPreviousOrders >= 3)
                discount += order.TotalAmount * 0.05m;
        }
        else if (order.CustomerCategory == Category.Regular)
        {
            /*
             But for regular customer, you use the average value of their purchase history total amount 
            which should be more than 500 to attract a discount of 10% 
            */
            if (avgPrevious > 500)
                discount += order.TotalAmount * 0.10m;
        }

        return discount;
    }
}