namespace Company.Consumers
{
    using MassTransit;

    public class GamePlayerConsumerDefinition :
        ConsumerDefinition<GamePlayerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GamePlayerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}