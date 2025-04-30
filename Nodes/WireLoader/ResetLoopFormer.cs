using BehaviorTrees;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace Nodes.WireLoader;

[DataContract]
[BTNode("ResetLoopFormer", "Wire Loader")]
public class ResetLoopFormer : Node
{
	bool _completed;

	ILogger<ResetLoopFormer>? _logger;
	private ILogger<ResetLoopFormer> Logger
	{
		get
		{
			_logger ??= ServiceProvider.GetService<ILoggerFactory>()!.CreateLogger<ResetLoopFormer>();
			return _logger;
		}
	}

	protected override void OnActivated()
	{
		base.OnActivated();
		Logger.LogInformation($"{GetType().Name}: resetting loop former");
		Task.Delay(1000).ContinueWith(_ =>
		{
			Logger.LogInformation($"{GetType().Name}: loop former has been reset");
			_completed = true;
		});
	}

	protected override ExecutingStatus OnExecuted()
	{
		return _completed ? ExecutingStatus.Success : ExecutingStatus.Running;
	}
}