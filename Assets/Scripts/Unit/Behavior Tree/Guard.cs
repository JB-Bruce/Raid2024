using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

/// <summary>
/// Get new point in the guard area like target for the unit
/// </summary>

public class Guard : Node
{
    UnitMovement _unit;

    public Guard(GameObject unit)
    {
        _unit = unit.GetComponent<UnitMovement>();
    }

    public override NodeState Evaluate()
    {
        _unit.SetGuardPoint(Vector3.zero, 0, 10); // Suppr this and set when you create the unit 
        Vector3 target = _unit.GetRandomPointOnGuardPoint();
        _unit.ChangeTarget(target);
        return NodeState.SUCCESS;
    }
}
