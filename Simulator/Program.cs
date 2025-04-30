using BehaviorTrees.Engine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simulator;

Console.WriteLine("Behavior tree example");

var host = new HostBuilder()
	.ConfigureServices((hostContext, services) =>
	{
		services.AddSingleton<Engine>();
		services.AddHostedService<SimulatorService>();
		services.AddLogging(loggingBuilder => loggingBuilder
			.AddSimpleConsole()
			.SetMinimumLevel(LogLevel.Trace));
	})
	.Build();

ILogger<Program> logger = host.Services.GetService<ILogger<Program>>()!;

logger.LogInformation($"Starting Behavior tree example");
await host.RunAsync();
logger.LogInformation($"Quitting Behavior tree example");