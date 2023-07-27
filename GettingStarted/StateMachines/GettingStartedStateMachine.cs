namespace StateMachines;

using Contracts;
using MassTransit;

public class GettingStartedStateMachine :
    MassTransitStateMachine<GettingStartedState> 
{
    public GettingStartedStateMachine()
    {
        InstanceState(x => x.CurrentState, Created);

        Event(() => GettingStartedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

        Initially(
            When(GettingStartedEvent)
                .Then(context => context.Instance.Value = context.Data.Value)
                .TransitionTo(Created)
        );

        SetCompletedWhenFinalized();
    }

    public State Created { get; private set; }

    public Event<GettingStartedEvent> GettingStartedEvent { get; private set; }
}