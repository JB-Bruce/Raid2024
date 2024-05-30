using UnityEngine;
using static Node_script;

/// <summary>
/// Get new point on Map like target for the unit
/// </summary>

public class Patrol : Node
{
    UnitMovement _unit;

    public Patrol(GameObject unit)
    {
        _unit = unit.GetComponent<UnitMovement>();
    }

    public override NodeState Evaluate()
    {
        Vector3 target = _unit.GetRandomPointOnMap();
        _unit.ChangeTarget(target);
        return NodeState.SUCCESS;
    }
}
