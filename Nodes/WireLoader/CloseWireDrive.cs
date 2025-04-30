using BehaviorTrees;
using System.Runtime.Serialization;
using BehaviorTrees.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nodes.WireLoader;

[DataContract]
[BTNode("CloseWireDrive", "Wire Loader")]
public class CloseWireDrive : Node
{
	bool _completed;

	ILogger<CloseWireDrive>? _logger;
	private ILogger<CloseWireDrive> Logger
	{
		get
		{
			_logger ??= ServiceProvider.GetService<ILoggerFactory>()!.CreateLogger<CloseWireDrive>();
			return _logger;
		}
	}

	protected override void OnActivated()
	{
		base.OnActivated();
		Logger.LogInformation($"{GetType().Name}: closing wire drive");
		Task.Delay(1000).ContinueWith(_ =>
		{
			WireLoadedEvent ev = new ();
			ev.Owner = Owner;
			EventManager.Instance.TriggerEvent(ev);

			Logger.LogInformation($"{GetType().Name}: wire drive closed");
			_completed = true;
		});
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}

public class WireLoadedEvent: BaseEvent
{
	public override bool Check(BaseEvent e)
	{
		WireLoadedEvent? wireLoadedEvent = e as WireLoadedEvent;
		if (wireLoadedEvent != null)
		{
			return true;
		}

		return false;
	}
}