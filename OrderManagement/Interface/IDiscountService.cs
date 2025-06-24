using OrderManagement.Data.Entity;
using OrderManagement.Models;

namespace OrderManagement.Interface;

public interface IDiscountService
{
    decimal CalculateDiscount(OrderDto order);
}