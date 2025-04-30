using BehaviorTrees.Engine;
using BehaviorTrees.Utils;
using BehaviorTrees;
using Microsoft.Extensions.Hosting;

namespace Simulator;

public class SimulatorService(Engine engine, IServiceProvider serviceProvider) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		BTScript _script = BTScript.Load("Examples/wire-loader.btree");
		engine.LoadScene(new List<Entity> { new Entity("Actor1") }, _script);
		Entity? entity = engine.Entities.FirstOrDefault();
		Node? instance = Reflector.Clone(_script.BehaviorTree);

		engine.ExecuteScript(instance, entity);
	}
}
