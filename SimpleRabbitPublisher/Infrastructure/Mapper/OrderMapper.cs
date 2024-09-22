using Riok.Mapperly.Abstractions;
using SimpleRabbitPublisher.DTOs;
using SimpleRabbitPublisher.Models;

namespace SimpleRabbitPublisher.Infrastructure.Mapper;

[Mapper]
public partial class OrderMapper
{
    public partial OrderResult OrderToOrderResponse(Order order);
    public partial Order OrderInputToOrder(OrderInput order);
}
