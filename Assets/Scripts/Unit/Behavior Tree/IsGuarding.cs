using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

/// <summary>
/// Is the state of the Unit AreaGuard
/// </summary>
public class IsGuarding : Node
{
    private UnitBT _tree;

    public IsGuarding(UnitBT tree)
    {
        _tree = tree;
    }

    public override NodeState Evaluate()
    {
        if (_tree.order == UnitOrder.AreaGuard)
        {
            Debug.Log("Guard");
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
