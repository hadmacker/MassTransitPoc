namespace Contracts
{
    using System;

    public record GettingStartedEvent
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }

    public record StartGameEvent
    {
        public Guid CorrelationId { get; init; }
    }

    public record GuessEvent
    {
        public Guid CorrelationId { get; init; }
        public int GuessValue { get; init; }
    }

    public record WrongGuessEvent
    {
        public Guid CorrelationId { get; init; }
        public int WrongAttempts { get; init; }
    }

    public record CorrectGuessEvent
    {
        public Guid CorrelationId { get; init; }
    }
}