namespace Contracts
{
    using System;

    public record MassTransitPocEvent
    {
        public Guid CorrelationId { get; init; }
        public string Value { get; init; }
    }
}