namespace GettingStarted.Contracts
{
    using System;

    public record GameRuleEvent
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; init; }
    }
}