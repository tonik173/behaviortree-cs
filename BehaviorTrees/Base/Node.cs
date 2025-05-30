// Copyright(c) 2015 Eugeny Novikov. Code under MIT license.

using BehaviorTrees.Collections;
using BehaviorTrees.Engine;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace BehaviorTrees
{
	/// <summary>
	/// Notify about changing. Need for nested structures, components (not only for entities).
	/// </summary>
	public interface INotified
	{
		void Notify(object sender, object eventArgs);
	}

	/// <summary>
	/// The node is the most fundamental concept. It's like a Lego brick. You build behaviors by
	/// combining many different nodes. it carries out a task that can either succeed or fail.
	/// </summary>
	[DataContract]
	public abstract class Node : TreeNode<Node>, IScriptedContextSupport, INotified
	{
		protected Entity _owner;
		protected IScriptedContext _scriptedContext;
		private bool _isActive;
		protected IServiceProvider ServiceProvider => _owner?.ServiceProvider;

		/// <summary>
		/// 
		/// </summary>
		public bool IsActive
		{
			get => _isActive;
			set
			{
				if (_isActive == value)
					return;

				_isActive = value;

				if (_isActive)
					Activate();
				else
					Deactivate();
			}
		}


		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public Entity Owner
		{
			get => _owner;
			set
			{
				if (_owner == value)
					return;

				if (_owner != null)
					OnDetachedFromOwner(_owner);

				_owner = value;

				if (_owner != null)
					OnAttachedToOwner(_owner);

				foreach (var node in Nodes)
					node.Owner = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public IScriptedContext ScriptedContext
		{
			get => _scriptedContext;
			set
			{
				if (_scriptedContext != value)
				{
					_scriptedContext = value;
					OnScriptedContextChanged(_scriptedContext);
				}

				foreach (var node in Nodes)
					node.ScriptedContext = value;
			}
		}

		[Browsable(false)]
		public virtual string NodeType => GetType(GetType());

		[Browsable(false)]
		public virtual string NodeName => GetName(GetType());

		[Browsable(false)]
		public virtual string NodeParameters => "";

		/// <summary>
		/// 
		/// </summary>
		public Node()
		{
			_isActive = false;
		}

		public void Activate()
		{
			if (IsActive)
				return;
			IsActive = true;
			OnActivated();

			if (IsRoot)
				OnRootActivated();
		}

		public void Deactivate()
		{
			if (!IsActive)
				return;
			IsActive = false;
			OnDeactivated();

			if (IsRoot)
				OnRootDeactivated();
		}

		public ExecutingStatus Execute()
		{
			if (!IsActive)
				Activate();

			var status = OnExecuted();

			if (status != ExecutingStatus.Running)
				Deactivate();

			return status;
		}

		public virtual void Notify(object sender, object eventArgs)
		{
			Root.SendValueChanged(this);
		}

		protected virtual void OnRootActivated()
		{
			foreach (var node in Nodes)
				node.OnRootActivated();
		}

		protected virtual void OnRootDeactivated()
		{
			foreach (var node in Nodes)
				node.OnRootDeactivated();
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnActivated()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void OnDeactivated()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual ExecutingStatus OnExecuted()
		{
			return ExecutingStatus.Success;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="newOwner"></param>
		protected virtual void OnAttachedToOwner(Entity newOwner)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="previousOwner"></param>
		protected virtual void OnDetachedFromOwner(Entity previousOwner)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		protected virtual void OnScriptedContextChanged(IScriptedContext context)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetName(Type type)
		{
			var attr = type.GetCustomAttribute<BTNodeAttribute>();
			return attr != null ? attr.Name : type.Name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetType(Type type)
		{
			var attr = type.GetCustomAttribute<BTNodeAttribute>();
			return attr != null ? attr.Type : "Node";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		protected override void OnSetParent(Node parent)
		{
			base.OnSetParent(parent);
			Owner = parent.Owner;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDetach()
		{
			base.OnDetach();
			Owner = null;
		}

		public override string ToString()
		{
			if (NodeParameters == "")
				return NodeName;
			else
				return NodeName + " - " + NodeParameters;
		}
	}
}
