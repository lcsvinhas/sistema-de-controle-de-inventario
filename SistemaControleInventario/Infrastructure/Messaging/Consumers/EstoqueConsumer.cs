using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SistemaControleInventario.Domain.Entities;
using SistemaControleInventario.Domain.Repositories;
using SistemaControleInventario.Infrastructure.Settings;
using System.Text;

namespace SistemaControleInventario.Infrastructure.Messaging.Consumers
{
    public class EstoqueConsumer : BackgroundService
    {
        private readonly ILogger<EstoqueConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQSettings _settings;
        private IConnection? _connection;
        private IModel? _channel;

        public EstoqueConsumer(
            ILogger<EstoqueConsumer> logger,
            IOptions<RabbitMQSettings> options,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _settings = options.Value;
            _serviceProvider = serviceProvider;
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "estoque_baixo",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _channel.QueueDeclare(queue: "produto_atualizado",
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InitRabbitMQ();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var produto = JsonConvert.DeserializeObject<Produto>(message);
                    if (produto == null)
                    {
                        _logger.LogWarning("Mensagem recebida mas não pôde ser desserializada em Produto.");
                        _channel.BasicAck(ea.DeliveryTag, false);
                        return;
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<INotificacaoRepository>();

                    if (ea.RoutingKey == "estoque_baixo")
                    {
                        _logger.LogInformation($"Alerta: Estoque baixo do produto {produto.Nome} - Estoque atual: {produto.Estoque}");

                        var notificacao = new Notificacao($"Estoque baixo: {produto.Nome} (Estoque atual: {produto.Estoque})");
                        await repo.Save(notificacao);
                    }
                    else if (ea.RoutingKey == "produto_atualizado")
                    {
                        _logger.LogInformation($"Produto atualizado: {produto.Nome}");

                        var notificacao = new Notificacao($"Produto atualizado: {produto.Nome}");
                        await repo.Save(notificacao);
                    }
                    else
                    {
                        _logger.LogWarning($"Mensagem recebida com routing key inesperada: {ea.RoutingKey}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ.");
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: "estoque_baixo", autoAck: false, consumer: consumer);
            _channel.BasicConsume(queue: "produto_atualizado", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
