using BehaviorTrees;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace Nodes;


[DataContract]
[BTNode("Hello", "Examples")]
public class Hello : Node
{
	private string _firstName;
	private bool _completed;

	ILogger<Hello>? _logger;
	private ILogger<Hello> Logger
	{
		get
		{
			_logger ??= ServiceProvider.GetService<ILoggerFactory>()!.CreateLogger<Hello>();
			return _logger;
		}
	}

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
		Logger.LogInformation($"{GetType().Name} says hello to {FirstName}");
		Task.Delay(1000).ContinueWith(_ => _completed = true);
	}

	protected override void OnDeactivated()
	{
		base.OnDeactivated();
		Logger.LogInformation($"{GetType().Name} greeting completed ");
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}