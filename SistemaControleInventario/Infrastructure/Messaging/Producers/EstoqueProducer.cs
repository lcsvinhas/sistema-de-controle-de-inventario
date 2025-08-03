using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Infrastructure.Messaging.Producers;
using SistemaControleInventario.Infrastructure.Settings;
using System.Text;

public class EstoqueProducer : IEstoqueProducer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public EstoqueProducer(IOptions<RabbitMQSettings> options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.HostName,
            UserName = options.Value.UserName,
            Password = options.Value.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "estoque_baixo", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public Task EnviarMensagemEstoqueBaixo(Produto produto)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(produto));

        _channel.BasicPublish(exchange: "", routingKey: "estoque_baixo", basicProperties: null, body: body);
        return Task.CompletedTask;
    }

    public Task EnviarMensagemProdutoAtualizado(Produto produto)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(produto));

        _channel.BasicPublish(exchange: "", routingKey: "produto_atualizado", basicProperties: null, body: body);
        return Task.CompletedTask;
    }
}
