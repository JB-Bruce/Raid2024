using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

public class GoToSurveillancePoint : Node_script.Node
{
    private UnitBT _unit;
    private UnitMovement _unitMove;
    private NavMeshAgent _navMesh;

    public GoToSurveillancePoint(UnitMovement unitMove, UnitBT unit)
    {
        _unit = unit;
        _unitMove = unitMove;
        _navMesh = unit.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        if (_unitMove.transform.position != _unit.surveillancePoint && _navMesh.pathEndPosition != _unit.surveillancePoint)
        {
            _unitMove.ChangeTarget(_unit.surveillancePoint);
        }
        return NodeState.SUCCESS;
    }
}
