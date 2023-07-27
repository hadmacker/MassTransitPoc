namespace StateMachines;

using System;
using MassTransit;
using Contracts;
using GettingStarted.Services;

public class GameSaga : MassTransitStateMachine<GameSagaState>
{
    public Event<StartGameEvent> StartGame { get; private set; }
    public Event<GuessEvent> Guess { get; private set; }

    public State Correct { get; private set; }
    public State Wrong { get; private set; }

    public GameSaga(IAnswerService answerService)
    {
        InstanceState(x => x.CurrentState);

        Event(() => StartGame, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => Guess, x => x.CorrelateById(context => context.Message.CorrelationId));

        Initially(
            When(StartGame)
                .Then(context => {
                    Console.WriteLine("GameSaga initiated from StartGameEvent");
                    context.Saga.Answer = answerService.GetAnswer();
                    })
                .TransitionTo(Wrong)
        );

        During(Wrong,
            When(Guess)
                .Then(context => {
                    if (IsGuessCorrect(context))
                    {
                        context.Saga.WrongAttempts = 0;
                        context.TransitionToState(Correct);
                        Console.WriteLine("Correct!");
                        context.Publish(new CorrectGuessEvent { CorrelationId = context.Message.CorrelationId });
                    }
                    else
                    {
                        context.Saga.WrongAttempts++;
                        if (context.Saga.WrongAttempts >= 6)
                        {
                            Console.WriteLine("Wrong, too many attempts!");
                            context.TransitionToState(Final);
                            context.Publish(new WrongGuessEvent { CorrelationId = context.Message.CorrelationId, WrongAttempts = context.Saga.WrongAttempts });
                        }
                        else
                        {
                            Console.WriteLine("Wrong, try again!");
                            context.TransitionToState(Wrong);
                            context.Publish(new WrongGuessEvent { CorrelationId = context.Message.CorrelationId, WrongAttempts = context.Saga.WrongAttempts });
                        }
                    }
                })
        );

        During(Correct,
            When(Guess)
                .Then(context => Console.WriteLine("Correct, then guess?"))
                .Publish(context => new CorrectGuessEvent { CorrelationId = context.Message.CorrelationId })
        );
    }

    private void HandleGuess(BehaviorContext<GameSagaState, GuessEvent> context)
    {
        // Perform any processing or validation related to the guess event.
    }

    private bool IsGuessCorrect(BehaviorContext<GameSagaState, GuessEvent> context)
    {
        // Determine if the guess is correct based on the game logic.
        // For demonstration purposes, assume it's correct if the guess is greater than 5.
        return context.Message.GuessValue > 4;
    }
}