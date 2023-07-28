namespace GettingStarted.Contracts
{
    using System;

    public record GuessEvent
    {
        public Guid CorrelationId { get; init; }
        public string GuessValue { get; set; }
    }
}