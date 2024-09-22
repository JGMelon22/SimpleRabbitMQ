using Microsoft.AspNetCore.Mvc;
using SimpleRabbitPublisher.DTOs;
using SimpleRabbitPublisher.Interfaces;

namespace SimpleRabbitPublisher.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMessagePublisher _messagePublisher;

    public OrdersController(IOrderRepository orderRepository, IMessagePublisher messagePublisher)
    {
        _orderRepository = orderRepository;
        _messagePublisher = messagePublisher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        return orders.Data != null && orders.Data.Any()
            ? Ok(orders)
            : NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        return order.Data != null
            ? Ok(order)
            : NotFound(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewOrderAsync([FromBody] OrderInput newOrder)
    {
        var order = await _orderRepository.AddOrderAsync(newOrder);

        if (order.Data != 0)
        {
            _messagePublisher.SendMessage(order);
            return Ok(order);
        }

        return BadRequest(order);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateOrderAsync([FromRoute] int id, [FromBody] OrderInput updatedOrder)
    {
        var order = await _orderRepository.UpdateOrderAsync(id, updatedOrder);

        if (order.Data != 0)
        {
            _messagePublisher.SendMessage(order);
            return NoContent();
        }
        return NotFound(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> UpdateOrderAsync([FromRoute] int id)
    {
        var order = await _orderRepository.RemoveOrderAsync(id);

        if (order.Data != 0)
        {
            _messagePublisher.SendMessage(order);
            return NoContent();
        }

        return NotFound(order);
    }
}
