using GShopping.MessageBus;

namespace GShopping.CartAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage (BaseMessage BaseMessage, string queueName);
    }
}
