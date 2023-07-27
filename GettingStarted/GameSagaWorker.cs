namespace Game;

using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

public class GameSagaWorker : BackgroundService
{
    readonly IBus _bus;

    public GameSagaWorker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var correlationId = Guid.NewGuid();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(2000, stoppingToken);

            await _bus.Publish(new StartGameEvent{ CorrelationId = correlationId }, stoppingToken);

            await Task.Delay(1000, stoppingToken);

            for (int i = 1; i <= 5; i++)
            {
                var guessEvent = new GuessEvent { CorrelationId = correlationId, GuessValue = i };
                await _bus.Publish(guessEvent);
                await Task.Delay(1000, stoppingToken);
                // Instead of above that just guesses in order, lets have a consumer listen for a "guess my number" event published by the saga.
                // Allow the worker to try and guess, using a consumer.
                // https://masstransit.io/documentation/concepts/consumers
            }
            return;
        }
    }
}