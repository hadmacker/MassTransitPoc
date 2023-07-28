namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System;

    public class GamePlayerConsumer :
        IConsumer<WrongGuessEvent>,
        IConsumer<GameStartedEvent>,
        IConsumer<GameOverEvent>,
        IConsumer<GameRuleEvent>,
        IConsumer<CorrectGuessEvent>

    {
        private static int NextGuessPosition = 0;
        private static List<string> Letters = new List<string>
        {
            "R","error","S","T","L","N","E","A","B","C","D","F","G","H","I","J","K","M","O","P","Q","U","V","W","X","Y","Z"
        };

        private static string NextGuess()
        {
            var nextGuess = Letters[NextGuessPosition];
            NextGuessPosition++;
            if (NextGuessPosition >= Letters.Count)
                NextGuessPosition = 0;
            return nextGuess;
        }

        public GamePlayerConsumer(ILogger<GamePlayerConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<WrongGuessEvent> context)
        {
            var allGuesses = string.Join(',', context.Message.AllGuesses);
            Console.WriteLine($"Wrong Guess:        {context.Message.MaskedAnswer} ({context.Message.WrongAttempts}) {allGuesses}");

            var nextGuess = NextGuess();
            Console.WriteLine($"Next Guess: {nextGuess}");
            await context.Publish<GuessEvent>(new
            {
                CorrelationId = context.Message.CorrelationId,
                GuessValue = nextGuess
            });
        }

        public async Task Consume(ConsumeContext<GameStartedEvent> context)
        {
            Console.WriteLine($"GAME STARTED! Word: {context.Message.MaskedAnswer} ({context.Message.MaskedAnswer.Length} characters)");

            var nextGuess = NextGuess();
            Console.WriteLine($"Next Guess: {nextGuess}");
            await context.Publish<GuessEvent>(new
            {
                CorrelationId = context.Message.CorrelationId,
                GuessValue = nextGuess
            });
        }

        public async Task Consume(ConsumeContext<GameOverEvent> context)
        {
            Console.WriteLine($"GAME OVER! Word:    {context.Message.MaskedAnswer} ({context.Message.MaskedAnswer.Length} characters) after {context.Message.AllGuesses.Count} guesses");
        }

        public async Task Consume(ConsumeContext<GameRuleEvent> context)
        {
            Console.WriteLine($"GAME RULE ERROR! {context.Message.Message}");

            var nextGuess = NextGuess();
            Console.WriteLine($"Next Guess: {nextGuess}");
            await context.Publish<GuessEvent>(new
            {
                CorrelationId = context.Message.CorrelationId,
                GuessValue = nextGuess
            });
        }

        public async Task Consume(ConsumeContext<CorrectGuessEvent> context)
        {
            Console.WriteLine($"Correct! Word:      {context.Message.MaskedAnswer} ({context.Message.MaskedAnswer.Length} characters)");

            var nextGuess = NextGuess();
            Console.WriteLine($"Next Guess: {nextGuess}");
            await context.Publish<GuessEvent>(new
            {
                CorrelationId = context.Message.CorrelationId,
                GuessValue = nextGuess
            });
        }
    }
}