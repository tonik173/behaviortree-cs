using BehaviorTrees;
using BehaviorTrees.Utils;
using System.Runtime.Serialization;

namespace Nodes;


[DataContract]
[BTNode("Hello", "Examples")]
public class Hello : Node
{
	private string _firstName;
	private bool _completed;

	[DataMember]
	public string FirstName
	{
		get => _firstName;
		set
		{
			_firstName = value;
			Root.SendValueChanged(this);
		}
	}

	public Hello()
	{ }

	public Hello(string firstName)
	{
		FirstName = firstName;
	}

	public override string NodeParameters => $"Says hello {FirstName})";

	protected override void OnActivated()
	{
		base.OnActivated();
		Log.Write($"{GetType().Name} says hello to {FirstName}");
		Task.Delay(1000).ContinueWith(_ => _completed = true);
	}

	protected override void OnDeactivated()
	{
		base.OnDeactivated();
		Log.Write($"{GetType().Name} greeting completed ");
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}