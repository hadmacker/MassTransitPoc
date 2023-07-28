namespace Contracts
{
    using System;

    public record GameStartedEvent
    {
        public Guid CorrelationId { get; init; }

        public string MaskedAnswer { get; init; }
    }
}