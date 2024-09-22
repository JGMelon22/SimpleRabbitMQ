using System.Data;
using Dapper;
using SimpleRabbitPublisher.DTOs;
using SimpleRabbitPublisher.Infrastructure.Mapper;
using SimpleRabbitPublisher.Interfaces;
using SimpleRabbitPublisher.Models;

namespace SimpleRabbitPublisher.Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IDbConnection _dbConnection;

    public OrderRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ServiceResponse<int>> AddOrderAsync(OrderInput newOrder)
    {
        var serviceResponse = new ServiceResponse<int>();
        var orderMapper = new OrderMapper();
        try
        {
            string sql = """
                        INSERT INTO rabbitmq_orders.orders(product_name, price, quantity, registered_date)
                        VALUES(@ProductName, @Price, @Quantity, @RegisteredDate);
                        """;

            _dbConnection.Open();

            var order = orderMapper.OrderInputToOrder(newOrder);
            var orderResult = await _dbConnection.ExecuteAsync(sql, order);

            if (orderResult == 0)
                throw new Exception("An error ocurred while inserting a new register");

            serviceResponse.Data = orderResult;
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        finally
        {
            _dbConnection.Close();
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<ICollection<OrderResult>>> GetAllOrdersAsync()
    {
        var serviceResponse = new ServiceResponse<ICollection<OrderResult>>();
        var orderMapper = new OrderMapper();

        try
        {
            string sql = """
                        SELECT order_id AS Id,
                               product_name ProductName,
                               price AS Price,
                               quantity AS Quantity,
                               registered_date AS RegisteredDate
                        FROM orders;
                        """;

            _dbConnection.Open();

            var orders = await _dbConnection.QueryAsync<Order>(sql);
            var ordersResult = orders.Select(x => orderMapper.OrderToOrderResponse(x)).ToList();

            serviceResponse.Data = ordersResult;
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        finally
        {
            _dbConnection.Close();
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<OrderResult>> GetOrderByIdAsync(int id)
    {
        var serviceResponse = new ServiceResponse<OrderResult>();
        var orderMapper = new OrderMapper();

        try
        {
            string sql = """
                        SELECT order_id AS Id,
                               product_name ProductName,
                               price AS Price,
                               quantity AS Quantity,
                               registered_date AS RegisteredDate
                        FROM orders
                        WHERE order_id = @Id
                        """;

            var order = await _dbConnection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
            var orderResult = order
                ?? throw new Exception($"Order with id {id} not found!");

            serviceResponse.Data = orderMapper.OrderToOrderResponse(orderResult);
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        finally
        {
            _dbConnection.Close();
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<int>> RemoveOrderAsync(int id)
    {
        var serviceResponse = new ServiceResponse<int>();
        var orderMapper = new OrderMapper();

        try
        {
            string sql = """
                        DELETE FROM orders
                        WHERE order_id = @Id
                        """;

            _dbConnection.Open();
            var orderResult = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            if (orderResult == 0)
                throw new Exception($"Video Game with id {id} not found!");

            serviceResponse.Data = orderResult;
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        finally
        {
            _dbConnection.Close();
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<int>> UpdateOrderAsync(int id, OrderInput updatedOrder)
    {
        var serviceResponse = new ServiceResponse<int>();
        var orderMapper = new OrderMapper();

        try
        {
            string sql = """
                        UPDATE orders
                        SET product_name = @ProductName,
                            price = @Price,
                            quantity = @Quantity,
                            registered_date = @RegisteredDate
                        WHERE order_id = @Id;
                        """;

            _dbConnection.Open();

            var order = orderMapper.OrderInputToOrder(updatedOrder);
            order.Id = id;

            var orderResult = await _dbConnection.ExecuteAsync(sql, order);

            if (orderResult == 0)
                throw new Exception($"Order with id {id} not found!");

            serviceResponse.Data = orderResult;
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        finally
        {
            _dbConnection.Close();
        }

        return serviceResponse;
    }
}
