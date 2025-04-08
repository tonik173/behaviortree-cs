﻿using BehaviorTrees;
using BehaviorTrees.Utils;
using System.Runtime.Serialization;

namespace Nodes.WireLoader;


[DataContract]
[BTNode("MoveToSlot", "Wire Loader")]
public class MoveToSlot : Node
{
	int _slot;
	bool _completed;

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
		Log.Write($"{GetType().Name}: moving to slot {Slot}");
		Task.Delay(1500).ContinueWith(_ =>
		{
			Log.Write($"{GetType().Name}: at slot {Slot}");
			_completed = true;
		});
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}