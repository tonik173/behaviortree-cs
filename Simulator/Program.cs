using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Simulator;

Console.WriteLine("Behavior tree example");


var host = new HostBuilder()
	.ConfigureServices((hostContext, services) =>
	{
		services.AddHostedService<SimulatorService>();
	})
	.Build();

await host.RunAsync();


Console.WriteLine("Behavior tree example done");