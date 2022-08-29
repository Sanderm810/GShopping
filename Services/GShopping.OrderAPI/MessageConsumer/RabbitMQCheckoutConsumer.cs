using GShopping.OrderAPI.Messages;
using GShopping.OrderAPI.Model;
using GShopping.OrderAPI.RabbitMQSender;
using GShopping.OrderAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly OrderRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQCheckoutConsumer(OrderRepository repository, IRabbitMQMessageSender rabbitMQMessageSender)
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

            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);

            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (channel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CheckoutHeaderVO vo = JsonSerializer.Deserialize<CheckoutHeaderVO>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CheckoutHeaderVO vo)
        {
            OrderHeader order = new()
            {
                UserId = vo.UserId,
                FirstName = vo.FirstName,
                LastName = vo.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = vo.CardNumber,
                CouponCode = vo.CouponCode,
                CVV = vo.CVV,
                DiscountAmount = vo.DiscountAmount,
                Email = vo.Email,
                ExpiryMonthYear = vo.ExpiryMothYear,
                OrderTime = DateTime.Now,
                PurchaseAmount = vo.PurchaseAmount,
                PaymentStatus = false,
                Phone = vo.Phone,
                DateTime = vo.DateTime
            };

            foreach (var details in vo.CartDetails)
            {
                OrderProduct product = new()
                {
                    Id = details.Product.Id,
                    Description = details.Product.Description,
                    ImageUrl = details.Product.ImageUrl,
                    Name = details.Product.Name,
                    CategoryName = details.Product.CategoryName,
                    Price = details.Product.Price
                };
                OrderDetail detail = new()
                {
                    OrderProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Price = details.Product.Price,
                    Count = details.Count,
                    OrderProduct = product
                };
                order.CartTotalItems += details.Count;
                order.OrderDetails.Add(detail);
            }

            await _repository.AddOrder(order);

            vo.Id = order.Id;

            _rabbitMQMessageSender.SendMessage(vo, "emailqueue");
        }
    }
}
