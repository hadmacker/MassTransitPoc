namespace Contracts
{
    using System;
    using System.Collections.Generic;

    public record WrongGuessEvent
    {
        public Guid CorrelationId { get; init; }
        public int WrongAttempts { get; set; }
        public string MaskedAnswer { get; set; }
        public List<string> AllGuesses { get; set; }
    }
}