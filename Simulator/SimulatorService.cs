using BehaviorTrees.Engine;
using BehaviorTrees.Utils;
using BehaviorTrees;
using Microsoft.Extensions.Hosting;

namespace Simulator;

public class SimulatorService : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		Engine _engine = Engine.Instance;
		BTScript _script = BTScript.Load("Examples/wire-loader.btree");

		_engine.LoadScene(new List<Entity> { new Entity("Actor1") }, _script);
		Entity? entity = _engine.Entities.FirstOrDefault();
		Node? instance = Reflector.Clone(_script.BehaviorTree);

		_engine.ExecuteScript(instance, entity);
	}
}
