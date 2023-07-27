using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using StateMachines;
using Game;
using GettingStarted.Services;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddActivities(entryAssembly);

                        var azureConnStr = Environment.GetEnvironmentVariable("mtpoccs");
                        if(string.IsNullOrWhiteSpace(azureConnStr)) {
                            x.UsingInMemory((context, cfg) =>
                            {
                                cfg.ConfigureEndpoints(context);
                            });     
                        } else {
                            x.UsingAzureServiceBus((context,cfg) =>
                            {
                                cfg.Host(azureConnStr);

                                cfg.ConfigureEndpoints(context);
                            });
                        }
                    });

                    services.AddHostedService<TimeWorker>();
                    services.AddHostedService<GameSagaWorker>();
                    services.AddSingleton<Random>();
                    services.AddTransient<IAnswerService, EmbeddedAnswerService>();
                });
    }
}
