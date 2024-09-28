using System.Data;
using MySql.Data.MySqlClient;

namespace SimpleRabbitPublisher.Infrastructure.Data;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("Default")!;
    }

    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}
