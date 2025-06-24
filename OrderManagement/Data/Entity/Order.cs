namespace OrderManagement.Data.Entity;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public Category CustomerCategory { get; set; } = Category.Regular;
    public decimal TotalAmount { get; set; }
    public decimal DiscountApplied { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum Category
{
    Prestige,
    Regular
}
public enum OrderStatus
{
    Pending, 
    Processing,
    Delivered, 
    Rejected,
    Received,
    Cancelled
}