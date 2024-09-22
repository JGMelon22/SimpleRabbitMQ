using System.Data;
using MySql.Data.MySqlClient;
using SimpleRabbitPublisher.Infrastructure.Message;
using SimpleRabbitPublisher.Infrastructure.Repository;
using SimpleRabbitPublisher.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

# region [IDBConnection Registration]

builder.Services.AddScoped<IDbConnection>(x =>
    new MySqlConnection(builder.Configuration.GetConnectionString("Default"))
);

# endregion

# region [Repository Registration]

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

# endregion

# region [RabbitMQ Basic Registration]

builder.Services.AddScoped<IMessagePublisher, RabbitMQPublisher>();

# endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
