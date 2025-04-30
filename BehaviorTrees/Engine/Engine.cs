// Copyright(c) 2015 Eugeny Novikov. Code under MIT license.

using Microsoft.Extensions.Logging;

namespace BehaviorTrees.Engine
{
	public sealed class Engine(ILogger<Engine> logger, IServiceProvider serviceProvider)
	{
		public Action<object, EventArgs> SceneLoaded { get; set; }
		public Action<Node, EventArgs> ExecutionCompleted { get; set; }

		public bool IsDesignMode { get; internal set; }
		public List<Entity> Entities { get; set; } = new List<Entity>();
		public BTScript BTScript { get; set; }

		Node _currentTree;
		CancellationTokenSource _executionToken;

		public void LoadScene(IEnumerable<Entity> entities, BTScript script)
		{
			Entities.Clear();
			Entities.AddRange(entities);
			BTScript = script;

			SceneLoaded?.Invoke(this, EventArgs.Empty);
		}

		public void ExecuteScript(Node behaviorTree, Entity entity)
		{
			entity.ServiceProvider = serviceProvider;
			_currentTree = behaviorTree;
			_currentTree.Owner = entity;

			_executionToken = new CancellationTokenSource();

			Task.Run(() =>
			{
				while (!_executionToken.IsCancellationRequested &&
						_currentTree.Execute() == ExecutingStatus.Running)
				{ }
			}, _executionToken.Token).ContinueWith((t) =>
			{
				if (t.IsFaulted)
				{
					Exception ex = t.Exception;
					while (ex is AggregateException && ex.InnerException != null)
						ex = ex.InnerException;
					logger.LogError("The script execution failed", ex);
				}
				else if (t.IsCanceled)
				{
					logger.LogWarning("The script execution cancelled!");
				}
				else
				{
					logger.LogInformation("The script is executed successfully!");
				}

				if (!t.IsFaulted)
					_currentTree.Deactivate();
				ExecutionCompleted?.Invoke(_currentTree, EventArgs.Empty);
				_currentTree = null;

			}, TaskScheduler.Default);
		}

		public void StopScript()
		{
			if (_currentTree == null)
			{
				logger.LogInformation("_current == null");
				return;
			}

			_executionToken.Cancel();
		}
	}
}