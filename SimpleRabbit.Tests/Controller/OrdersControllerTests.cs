using SimpleRabbitPublisher.Controllers;
using SimpleRabbitPublisher.Interfaces;
using FakeItEasy;
using FluentAssertions;
using SimpleRabbitPublisher.DTOs;
using SimpleRabbitPublisher.Models;
using Microsoft.AspNetCore.Mvc;

namespace SimpleRabbit.Tests.Controllers;

public class OrdersControllersTests
{
    private readonly OrdersController _ordersController;
    private readonly IOrderRepository _orderRepository;
    private readonly IMessagePublisher _messagePublisher;

    public OrdersControllersTests()
    {
        _orderRepository = A.Fake<IOrderRepository>();
        _messagePublisher = A.Fake<IMessagePublisher>();

        _ordersController = new OrdersController(_orderRepository, _messagePublisher);
    }

    [Fact]
    public async Task When_AddNewOrderAsync_Should_Be_Ok()
    {
        // Arrange
        var newOrder = new OrderInput("Mastering C# and .NET Framework", 227.99M, 3, new DateTime(2016, 12, 15));
        var serviceResponse = new ServiceResponse<int>
        {
            Data = 1,
            Success = true,
            Message = string.Empty
        };

        A.CallTo(() => _orderRepository.AddOrderAsync(newOrder))
            .Returns(Task.FromResult(serviceResponse));

        A.CallTo(() => _messagePublisher.SendMessage(A<ServiceResponse<int>>._))
            .DoesNothing();

        // Act
        var result = await _ordersController.AddNewOrderAsync(newOrder);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Should().Be(1);
        serviceResponse.Success.Should().BeTrue();
        serviceResponse.Message.Should().BeEmpty();
        A.CallTo(() => _messagePublisher.SendMessage(serviceResponse))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task When_GetAllOrdersAsync_Should_Be_Ok()
    {
        // Arrange
        ICollection<OrderResult> orders = new List<OrderResult>
        {
            new() { Id = 1, ProductName = "Cook Book", Price = 25.88M, Quantity = 10, RegisteredDate = new DateTime(2024, 10, 12) },
            new() { Id = 2, ProductName = "Wireless Headphones", Price = 99.99M, Quantity = 15, RegisteredDate = new DateTime(2024, 7, 21) },
            new() { Id = 3, ProductName = "Gaming Laptop", Price = 1299.50M, Quantity = 5, RegisteredDate = new DateTime(2023, 12, 1) },
            new() { Id = 4, ProductName = "Smartwatch", Price = 199.99M, Quantity = 25, RegisteredDate = new DateTime(2024, 1, 18) },
            new() { Id = 5, ProductName = "Electric Kettle", Price = 45.75M, Quantity = 30, RegisteredDate = new DateTime(2024, 9, 15) }
        };

        var serviceResponse = new ServiceResponse<ICollection<OrderResult>>
        {
            Data = orders,
            Success = true,
            Message = string.Empty
        };

        A.CallTo(() => _orderRepository.GetAllOrdersAsync())
            .Returns(Task.FromResult(serviceResponse));

        // Act
        var result = await _ordersController.GetAllOrdersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Count().Should().Be(5);
        serviceResponse.Success.Should().BeTrue();
        serviceResponse.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task When_GetOrderByIdAsync_Should_Be_Ok()
    {
        // Arrange
        int id = 1;
        var orderResult = new OrderResult
        {
            Id = id,
            ProductName = "Ice Cream Machine",
            Price = 150.00M,
            Quantity = 6,
            RegisteredDate = new DateTime(2024, 09, 26)
        };

        var serviceResponse = new ServiceResponse<OrderResult>
        {
            Data = orderResult,
            Success = true,
            Message = string.Empty
        };

        A.CallTo(() => _orderRepository.GetOrderByIdAsync(id))
            .Returns(Task.FromResult(serviceResponse));

        // Act
        var result = await _ordersController.GetOrderByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        serviceResponse.Data.Id.Should().Be(1);
        serviceResponse.Data.ProductName.Should().Be("Ice Cream Machine");
        serviceResponse.Data.Price.Should().Be(150.00M);
        serviceResponse.Data.Quantity.Should().Be(6);
        serviceResponse.Data.RegisteredDate.Should().Be(new DateTime(2024, 09, 26));
        serviceResponse.Success.Should().BeTrue();
        serviceResponse.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task When_UpdateOrderAsync_Should_Be_NoContent()
    {
        // Arrange
        int id = 2;
        var updatedOrder = new OrderInput("Gaming Computer", 2000.50M, 2, new DateTime(2024, 09, 1));
        var serviceResponse = new ServiceResponse<int>
        {
            Data = 1,
            Success = true,
            Message = string.Empty
        };

        A.CallTo(() => _orderRepository.UpdateOrderAsync(id, updatedOrder))
            .Returns(Task.FromResult(serviceResponse));

        A.CallTo(() => _messagePublisher.SendMessage(A<ServiceResponse<int>>._))
            .DoesNothing();

        // Act
        var result = await _ordersController.UpdateOrderAsync(id, updatedOrder);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        serviceResponse.Data.Should().Be(1);
        serviceResponse.Success.Should().BeTrue();
        serviceResponse.Message.Should().BeEmpty();
        A.CallTo(() => _messagePublisher.SendMessage(serviceResponse))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task When_RemoveOrderAsync_Should_Be_NoContent()
    {
        // Arrange
        int id = 5;
        var serviceResponse = new ServiceResponse<int>
        {
            Data = 1,
            Success = true,
            Message = string.Empty
        };

        A.CallTo(() => _orderRepository.RemoveOrderAsync(id))
            .Returns(Task.FromResult(serviceResponse));

        A.CallTo(() => _messagePublisher.SendMessage(A<ServiceResponse<int>>._))
            .DoesNothing();

        // Act
        var result = await _ordersController.RemoveOrderAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
        serviceResponse.Data.Should().Be(1);
        serviceResponse.Success.Should().BeTrue();
        serviceResponse.Message.Should().BeEmpty();
        A.CallTo(() => _messagePublisher.SendMessage(serviceResponse))
            .MustHaveHappenedOnceExactly();
    }
}
