using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

/// <summary>
/// See if an enemy is in the area of the unit
/// </summary>

public class IsEnemyDetected : Node
{
    UnitCombat _unit;

    public IsEnemyDetected(GameObject unit)
    {
        _unit = unit.GetComponent<UnitCombat>();
    }

    public override NodeState Evaluate()
    {
        if (_unit.IsEnemyInMyArea() )
        {
            return NodeState.SUCCESS;
        }
        _unit.canAttack = false;
        return NodeState.FAILURE;
    }
}
