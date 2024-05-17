using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

/// <summary>
/// Is the state of the Unit AreaGuard
/// </summary>
public class IsGuarding : Node
{
    private Behavior_tree _tree;

    public IsGuarding(Behavior_tree tree)
    {
        _tree = tree;
    }

    public override NodeState Evaluate()
    {
        if (_tree.order == UnitOrder.AreaGuard)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
