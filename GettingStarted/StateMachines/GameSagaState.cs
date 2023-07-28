using System;
using System.Collections.Generic;
using System.Linq;
using MassTransit;

namespace GettingStarted.StateMachines
{
    public class GameSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public State CurrentState { get; set; }
        public int GuessValue { get; set; }

        public int WrongAttempts { get; set; }
        public string Answer { get; internal set; }
        public string MaskedAnswer { get; internal set; }
        public List<string> AllGuesses { get; internal set; }

        public bool ApplyGuess(string guess)
        {
            var guessChar = guess.ToLower()[0];
            if (!AllGuesses.Any(g => g.Equals(guessChar)))
                AllGuesses.Add(guessChar.ToString());

            var correctGuess = false;
            if (Answer.Any(c => c.Equals(guessChar)))
            {
                correctGuess = true;
                var maskedEditor = MaskedAnswer.ToArray();
                for (var i = 0; i < Answer.Length; i++)
                    if (Answer[i].Equals(guessChar))
                        maskedEditor[i] = guessChar;
                MaskedAnswer = new string(maskedEditor);
            }
            else
            {
                WrongAttempts++;
            }
            return correctGuess;
        }

        public void ApplyGameStart(string answer)
        {
            Answer = answer.ToLower();
            MaskedAnswer = new string('_', answer.Length);
            AllGuesses = new List<string>();
        }
    }
}