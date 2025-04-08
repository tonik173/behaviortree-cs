using BehaviorTrees.Utils;
using BehaviorTrees;
using System.Runtime.Serialization;
using BehaviorTrees.Engine;

namespace Nodes.WireLoader;

[DataContract]
[BTNode("CloseWireDrive", "Wire Loader")]
public class CloseWireDrive : Node
{
	bool _completed;


	protected override void OnActivated()
	{
		base.OnActivated();
		Log.Write($"{GetType().Name}: closing wire drive");
		Task.Delay(1000).ContinueWith(_ =>
		{
			WireLoadedEvent ev = new ();
			ev.Owner = Owner;
			EventManager.Instance.TriggerEvent(ev);

			Log.Write($"{GetType().Name}: wire drive closed");
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