using SimpleRabbitPublisher.DTOs;
using SimpleRabbitPublisher.Models;

namespace SimpleRabbitPublisher.Interfaces;

public interface IOrderRepository
{
    Task<ServiceResponse<ICollection<OrderResult>>> GetAllOrdersAsync();
    Task<ServiceResponse<OrderResult>> GetOrderByIdAsync(int id);
    Task<ServiceResponse<int>> AddOrderAsync(OrderInput newOrder);
    Task<ServiceResponse<int>> UpdateOrderAsync(int id, OrderInput updatedOrder);
    Task<ServiceResponse<int>> RemoveOrderAsync(int id);
}
