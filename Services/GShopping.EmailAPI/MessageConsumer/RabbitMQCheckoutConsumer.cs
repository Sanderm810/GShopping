using GShopping.EmailAPI.Email;
using GShopping.EmailAPI.Messages;
using GShopping.EmailAPI.Model;
using GShopping.EmailAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GShopping.EmailAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly EmailRepository _repository;
        private readonly IMailService mailService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCheckoutConsumer(EmailRepository repository, IMailService mailService)
        {
            _repository = repository;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "emailqueue", false, false, false, arguments: null);

            this.mailService = mailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutMessage vo = JsonSerializer.Deserialize<CheckoutMessage>(content);
                ProcessLogs(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("emailqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessLogs(CheckoutMessage message)
        {
            try
            {
                await _repository.LogEmail(message);
            }
            catch (Exception)
            {
                //Log
                throw;
            }

            //Email = message.Email,
            //    SendDate = DateTime.Now,
            //    Log = $"Pedido - {message.OrderId} foi criada com sucesso!",
            //    OrderId = message.OrderId

            MailRequest mailRequest = new MailRequest()
            {
                ToEmail = message.Email,
                Subject = $"Pedido - {message.Id}",
                Body = $"<h1>Pedido - {message.Id} </h1>"
            };
            await this.mailService.SendEmailAsync(mailRequest);
        }
    }
}
