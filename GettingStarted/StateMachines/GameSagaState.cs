namespace StateMachines;

using System;
using System.Collections.Generic;
using MassTransit;

public class GameSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; }
    public int GuessValue { get; set; }

    public int WrongAttempts {get; set;}
    public string Answer { get; internal set; }
    public List<string> Guesses { get; internal set; }
}