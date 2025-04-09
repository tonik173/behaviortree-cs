# Behavior Trees

A simple C# example of Behavior Trees + Editor based on the code provided by [Eugeny Novokov](https://github.com/EugenyN/BehaviorTrees).

## Basic nodes

### Composites

- **Selector**: The Selector will succeed as soon as a child node succeeds. The Selector will fail if all child nodes were tried and all of them failed.
- **Sequence**: The Sequence node, for instance, sequentially executes its children (its collection of nodes) in order. If all children succeeded we consider the Sequence itself to have succeeded. If, however, any of the children failed we immediately consider the Sequence itself to have failed, without proceeding to the next child node.
- **Parallel**: A parallel node runs it's children at the same time.
- **Trigger**: The Trigger node is a specialization of the Parallel node. It also listens for a BaseEvent and invokes the BaseEvent's Check method upon reception. It the returns the events check method result.

### Decorators

- **Deactivator**: Returns Success without subnode execution. Can be used for temporary disabling nodes.
- **Delay**: Execute decorated node after waiting certain amount of time.
- **ForceFailure**: Forced returns Failure, ignoring the result of the node, which is decorated.
- **ForceSuccess**: Forced returns Success, ignoring the result of the node, which is decorated.
- **Limiter**: Execute decorated node a specified amount of times and then returns Success.
- **Loop**: Loops decorated subnode a number of times, or infinitely.
- **RepeatAlways**: Executes infinitely regardless of the result.
- **Repeater**: Execute decorated node a specified amount of times, returning Running.
- **EventCondition**: Listens for a BaseEvent. Invokes BaseEvent Check method and returns its result.
- **DelegateCondition**: Node that can check some condition. Condition is given as  `bool Check(Entity arg)`.

### Actions

- **Action**: Executes given argument ActionBase upon activation.
- **DelegateAction**: Exceutes given delegate argument.
- **FailAction**: Always Failure.
- **FailAfterAction**: Return Failure after several iterations.
- **SucceedAction**: Always Success.
- **SucceedAfterAction**: Return Succeed after several iterations.

## Links

- [Example Behavior Tree using BehaviorTree.CPP](https://github.com/tonik173/behaviortree-example)
- [Introduction to Behavior Trees – YouTube playlist](https://www.youtube.com/playlist?list=PLFQdM4LOGDr_vYJuo8YTRcmv3FrwczdKg)
- [Robohub - Introduction to behavior trees](https://robohub.org/introduction-to-behavior-trees/)
- [Cornell University - Behavior Trees in Robotics and AI: An Introduction](https://arxiv.org/abs/1709.00084)