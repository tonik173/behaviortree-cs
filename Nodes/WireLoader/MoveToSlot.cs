using BehaviorTrees;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace Nodes.WireLoader;


[DataContract]
[BTNode("MoveToSlot", "Wire Loader")]
public class MoveToSlot : Node
{
	int _slot;
	bool _completed;

	ILogger<MoveToSlot>? _logger;
	private ILogger<MoveToSlot> Logger
	{
		get
		{
			_logger ??= ServiceProvider.GetService<ILoggerFactory>()!.CreateLogger<MoveToSlot>();
			return _logger;
		}
	}

	[DataMember]
	public int Slot
	{
		get { return _slot; }
		set
		{
			_slot = value;
			Root.SendValueChanged(this);
		}
	}

	public MoveToSlot()
	{ }

	public MoveToSlot(int slot)
	{
		Slot = slot;
	}

	public override string NodeParameters => $"Move To Slot ({Slot})";

	protected override void OnActivated()
	{
		
		base.OnActivated();
		Logger.LogInformation($"{GetType().Name}: moving to slot {Slot}");
		Task.Delay(1500).ContinueWith(_ =>
		{
			Logger.LogInformation($"{GetType().Name}: at slot {Slot}");
			_completed = true;
		});
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}