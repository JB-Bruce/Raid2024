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
        Vector3 target = _unit.GetRandomPointOnGuardPoint();
        _unit.ChangeTarget(target);
        return NodeState.SUCCESS;
    }
}
