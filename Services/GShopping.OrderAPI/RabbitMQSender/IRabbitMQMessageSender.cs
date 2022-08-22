using GShopping.MessageBus;

namespace GShopping.OrderAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage (BaseMessage BaseMessage, string queueName);
    }
}
