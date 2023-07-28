namespace StateMachines;

using System;
using MassTransit;
using Contracts;
using GettingStarted.Services;

public class GameSaga : MassTransitStateMachine<GameSagaState>
{
    public Event<StartGameEvent> StartGame { get; private set; }
    public Event<GuessEvent> Guess { get; private set; }
    public Event<GameOverEvent> Finalized { get; private set; }

    public State Final { get; private set; }
    public State Wrong { get; private set; }

    // Allow most puzzles to succeed, but with a few likely failures.
    const int maxWrongAttempts = 24;

    public GameSaga(IAnswerService answerService)
    {
        InstanceState(x => x.CurrentState);

        Event(() => StartGame, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => Guess, x => x.CorrelateById(context => context.Message.CorrelationId));

        Initially(
            When(StartGame)
                .Then(context =>
                {
                    Console.WriteLine("GameSaga initiated from StartGameEvent");
                    context.Saga.ApplyGameStart(answerService.GetAnswer());
                    context.Publish(
                        new GameStartedEvent
                        {
                            CorrelationId = context.Message.CorrelationId,
                            MaskedAnswer = context.Saga.MaskedAnswer
                        });
                })
                .TransitionTo(Wrong)
        );

        During(Wrong,
            When(Guess)
                .Then(context =>
                {
                    if (context.Message.GuessValue.Length != 1)
                    {
                        Console.WriteLine("Wrong, try again!");
                        context.TransitionToState(Wrong);
                        context.Publish(
                            new GameRuleEvent
                            {
                                CorrelationId = context.Message.CorrelationId,
                                Message = "GuessValue must be a single character"
                            });
                    }

                    var isCorrect = context.Saga.ApplyGuess(context.Message.GuessValue);
                    if (isCorrect)
                    {
                        Console.WriteLine("Correct!");

                        if (context.Saga.MaskedAnswer == context.Saga.Answer)
                        {
                            context.TransitionToState(Final);
                            context.Publish(
                                new GameOverEvent
                                {
                                    CorrelationId = context.Message.CorrelationId,
                                    WrongAttempts = context.Saga.WrongAttempts,
                                    MaskedAnswer = context.Saga.MaskedAnswer,
                                    AllGuesses = context.Saga.AllGuesses,
                                });
                        }
                        else
                        {
                            context.Publish(
                                new CorrectGuessEvent
                                {
                                    CorrelationId = context.Message.CorrelationId,
                                    MaskedAnswer = context.Saga.MaskedAnswer,
                                    AllGuesses = context.Saga.AllGuesses,
                                    WrongAttempts = context.Saga.WrongAttempts
                                });
                        }
                    }
                    else
                    {
                        if (context.Saga.WrongAttempts > maxWrongAttempts)
                        {
                            Console.WriteLine("Wrong, too many attempts!");
                            context.TransitionToState(Final);
                            context.Publish(
                                new GameOverEvent
                                {
                                    CorrelationId = context.Message.CorrelationId,
                                    WrongAttempts = context.Saga.WrongAttempts,
                                    MaskedAnswer = context.Saga.MaskedAnswer,
                                    AllGuesses = context.Saga.AllGuesses,
                                });
                        }
                        else
                        {
                            Console.WriteLine("Wrong, try again!");
                            context.TransitionToState(Wrong);
                            context.Publish(
                                new WrongGuessEvent
                                {
                                    CorrelationId = context.Message.CorrelationId,
                                    WrongAttempts = context.Saga.WrongAttempts,
                                    MaskedAnswer = context.Saga.MaskedAnswer,
                                    AllGuesses = context.Saga.AllGuesses,
                                });
                        }
                    }
                })
        );

        During(Final, When(Finalized).Finalize());

        SetCompletedWhenFinalized();
    }
}