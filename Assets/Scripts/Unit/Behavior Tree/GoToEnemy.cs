using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

public class GoToEnemy : Node
{
    UnitCombat _unit;
    UnitMovement _unitMovement;
    NavMeshAgent _agent;
    public GoToEnemy(GameObject unit)
    {
        _unit = unit.GetComponent<UnitCombat>();
        _unitMovement = unit.GetComponent<UnitMovement>();
        _agent = unit.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {

        if (((_agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.03f) || (/*_agent.pathStatus == NavMeshPathStatus.PathPartial &&*/ _agent.velocity == Vector3.zero))
            && _unit.nearestEnemy == null)
        {
            _unit.lastPosition = Vector3.zero;
            return NodeState.FAILURE;
        }

        if (_unit.nearestEnemy != null && _unit.weapon.AttackRange < Vector3.Distance(_unit.transform.position, _unit.nearestEnemy.transform.position))
        {
            _unitMovement.ChangeTarget(_unit.nearestEnemy.transform.position);
            return NodeState.SUCCESS;
        }

        else if(_unit.nearestEnemy == null && _unit.lastPosition != Vector3.zero && _unit.lastEnemy != null) 
        {
            _unitMovement.ChangeTarget(_unit.lastPosition);
            return NodeState.SUCCESS;
        }
        else 
        {
            _unit.lastPosition = Vector3.zero;
            _unitMovement.ChangeTarget(_unitMovement.transform.position);

        }



            return NodeState.FAILURE;
    }
}
