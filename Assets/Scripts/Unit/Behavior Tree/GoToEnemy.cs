using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node_script;

public class GoToEnemy : Node
{
    UnitCombat _unit;
    UnitMovement _unitMovement;
    public GoToEnemy(GameObject unit)
    {
        _unit = unit.GetComponent<UnitCombat>();
        _unitMovement = unit.GetComponent<UnitMovement>();
    }

    public override NodeState Evaluate()
    {
        GameObject enemy = _unit.GetNearrestEnemy();

        if (_unit.attackDistance < Vector3.Distance(_unit.transform.position, enemy.transform.position))
        {
            _unitMovement.ChangeTarget(enemy.transform.position);
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
