using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using static Node_script;

public class CanAttack : Node
{
    UnitCombat _unit;
    UnitMovement _unitMovement;
    public CanAttack(GameObject unit)
    {
        _unit = unit.GetComponent<UnitCombat>();
        _unitMovement = unit.GetComponent<UnitMovement>();
    }

    public override NodeState Evaluate()
    {
        GameObject enemy = _unit.GetNearrestEnemy();

        if(enemy != null && _unit.attackDistance >= Vector3.Distance(_unit.transform.position, enemy.transform.position))
        {
            _unitMovement.ChangeTarget(_unit.transform.position);
            _unit.canAttack = true;
            return NodeState.SUCCESS;
        }
        _unit.canAttack = false;
        return NodeState.FAILURE;
    }
}
