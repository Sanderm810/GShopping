namespace GShopping.MessageBus
{
    public interface IMessageBus
    {
        public Task PublicMessage(BaseMessage message, string queueName);
    }
}