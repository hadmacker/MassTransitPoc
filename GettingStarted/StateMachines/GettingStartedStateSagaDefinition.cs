namespace StateMachines;

using MassTransit;

public class GettingStartedStateSagaDefinition :
    SagaDefinition<GettingStartedState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<GettingStartedState> sagaConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}