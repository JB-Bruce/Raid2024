using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Node_script;

/// <summary>
/// Is the state of the Unit Patrol
/// </summary>
public class IsPatrolling : Node
{
    private Behavior_tree _tree;

    public IsPatrolling (Behavior_tree tree)
    {
        _tree = tree;
    }

    public override NodeState Evaluate()
    {
        if(_tree.order == UnitOrder.Patrol) 
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
