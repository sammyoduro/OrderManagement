using OrderManagement.Data.Entity;

namespace OrderManagement.Models;

public class OrderDto
{
   public string CustomerId { get; set; }
    public Category CustomerCategory { get; set; } 
    public decimal TotalAmount { get; set; }
    
    
}