﻿namespace GettingStarted.Contracts
{
    using System;

    public record StartGameEvent
    {
        public Guid CorrelationId { get; init; }
    }
}