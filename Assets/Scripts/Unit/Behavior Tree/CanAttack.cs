using UnityEngine;
using static Node_script;

public class CanAttack : Node
{
    UnitCombat _unit;
    UnitMovement _unitMovement;
    Transform _transform;
    public CanAttack(GameObject unit)
    {
        _unit = unit.GetComponent<UnitCombat>();
        _unitMovement = unit.GetComponent<UnitMovement>();
        _transform = _unit.transform;
    }

    public override NodeState Evaluate()
    {
        _unit.nearestEnemy = _unit.GetNearrestEnemy();

        if(_unit.nearestEnemy != null && _unit.weapon.AttackRange >= Vector3.Distance(_transform.position, _unit.nearestEnemy.transform.position))
        {
            _unitMovement.ChangeTarget(_transform.position);
            _unit.canAttack = true;
            return NodeState.SUCCESS;
        }
        _unit.canAttack = false;
        return NodeState.FAILURE;
    }
}
