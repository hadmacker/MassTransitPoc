namespace Game;

using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

public class StartGameSagaWorker : BackgroundService
{
    readonly IBus _bus;

    public StartGameSagaWorker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("GAME STARTER: Press 'S' to begin a new game.");
        while (!stoppingToken.IsCancellationRequested)
        {
            var keyInfo = Console.ReadKey();
            if (keyInfo.KeyChar == 's' || keyInfo.KeyChar == 'S')
            {
                Console.WriteLine("Starting new game");
                var correlationId = Guid.NewGuid();
                await _bus.Publish(new StartGameEvent { CorrelationId = correlationId }, stoppingToken);
            }
        }
    }
}
