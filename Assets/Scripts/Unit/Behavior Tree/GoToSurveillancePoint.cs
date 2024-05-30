using UnityEngine.AI;
using static Node_script;

public class GoToSurveillancePoint : Node_script.Node
{
    private UnitMovement _unit;
    private NavMeshAgent _navMesh;

    public GoToSurveillancePoint(UnitMovement unit)
    {
        _unit = unit;
        _navMesh = unit.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        if (_unit.transform.position != _unit.GetGuardPoint() && _navMesh.pathEndPosition != _unit.GetGuardPoint())
        {
            _unit.ChangeTarget(_unit.GetGuardPoint());
        }
        return NodeState.SUCCESS;
    }
}
