namespace GettingStarted.Contracts
{
    using System;
    using System.Collections.Generic;

    public record CorrectGuessEvent
    {
        public Guid CorrelationId { get; init; }
        public string MaskedAnswer { get; set; }
        public List<string> AllGuesses { get; set; }
        public int WrongAttempts { get; set; }
    }
}