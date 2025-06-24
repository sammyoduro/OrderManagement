using Microsoft.AspNetCore.Mvc;
using OrderManagement.Data.Entity;
using OrderManagement.Interface;
using OrderManagement.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace OrderManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("All-Orders")]
    [SwaggerOperation(Summary = "Get all orders", Description = "Returns a list of all orders.")]
    [SwaggerResponse(200, "Returns the list of orders")]
    public async Task<IActionResult> AllOrders()
    {
        var allOrders = await _orderService.GetAllOrdersAsync();
        return Ok(allOrders);
    }

    [HttpPost("New-Order")]
    [SwaggerOperation(Summary = "Create a new order")]
    [SwaggerResponse(201, "Order created successfully")]
    public async Task<IActionResult> NewOrder(OrderDto order)
    {
        var created = await _orderService.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
    }
    [HttpGet("Get-Order/{id}")]
    [SwaggerOperation(Summary = "Get a specific order by ID")]
    [SwaggerResponse(200, "Returns the order")]
    [SwaggerResponse(404, "Order not found")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        return Ok(await _orderService.GetOrderAsync(id));
    }
    
    [HttpPatch("Update-Order/{id}/{status}")]
    [SwaggerOperation(Summary = "Update the status of an order")]
    [SwaggerResponse(200, "Order status updated")]
    [SwaggerResponse(404, "Order not found")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] OrderStatus status)
    {
        var result = await _orderService.UpdateStatusAsync(id, status);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpDelete("Delete-Order/{id}")]
    [SwaggerOperation(Summary = "Delete an order", Description = "Returns success if oder is deleted else failed")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id)
    {
        var (result,message) = await _orderService.DeleteOrderAsync(id);
        return result ? Ok(message) : NotFound(new { message });
    }
    
    
    [HttpGet("analytics")]
    [SwaggerOperation(Summary = "Get analytics data", Description = "Returns average order value and average fulfillment time.")]
    public async Task<IActionResult> GetAnalytics()
    {
        var (avgValue, avgTime) = await _orderService.GetAnalyticsAsync();
        return Ok(new { avgValue, avgFulfillTimeInMinutes = avgTime });
    }
}