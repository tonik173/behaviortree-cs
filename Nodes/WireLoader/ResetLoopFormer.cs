using BehaviorTrees;
using BehaviorTrees.Utils;
using System.Runtime.Serialization;

namespace Nodes.WireLoader;


[DataContract]
[BTNode("ResetLoopFormer", "Wire Loader")]
public class ResetLoopFormer : Node
{
	bool _completed;

	protected override void OnActivated()
	{
		base.OnActivated();
		Log.Write($"{GetType().Name}: resetting loop former");
		Task.Delay(1000).ContinueWith(_ =>
		{
			Log.Write($"{GetType().Name}: loop former has been reset");
			_completed = true;
		});
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}