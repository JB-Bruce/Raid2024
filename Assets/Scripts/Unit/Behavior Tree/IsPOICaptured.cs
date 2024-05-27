using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;
using static UnityEngine.UI.CanvasScaler;
public class IsPOICaptured : Node
{
    UnitMovement _unit;
    Humanoid _humanoid;
    UnitBT _unitBT;

    public IsPOICaptured(UnitMovement unit)
    {
        _unit = unit;
        _humanoid = unit.gameObject.GetComponent<Humanoid>();
        _unitBT = unit.gameObject.GetComponent<UnitBT>();
    }

    public override NodeState Evaluate()
    {
        if(_unit.targetPOI.ownerFaction == _humanoid.faction && _unit.targetPOI.capturePercentage == 100)
        {
            _unit.poiHasBeenCaptured = true;
            _unit.SetGuardPoint(_unit.targetPOI.poiPosition.position, _unit.targetPOI.minDistancePOI, _unit.targetPOI.maxDistancePOI);
            return NodeState.FAILURE;
        }

        if(AlliesHaveCapturedPOI())
        {
            _unitBT.order = UnitOrder.Patrol;
            for (int i = 0; i < FactionManager.Instance.factions.Count; i++)
            {
                if (FactionManager.Instance.factions[i].faction == _unitBT.faction)
                {
                    FactionManager.Instance.factions[i].RemoveJob(_unit.gameObject);
                }
            }
        }

        if(_unit.poiHasBeenCaptured)
        {
            _unit.poiHasBeenCaptured = false;
            _unit.SetGuardPoint(_unit.targetPOI.transform.position, 0, _unit.gameObject.GetComponent<CircleCollider2D>().radius);
            _unit.SetGuardPoint(_unit.GetRandomPointOnGuardPoint(), 0, 0);
        }
            return NodeState.SUCCESS;
    }

    // Return true if an ally captured the POI
    private bool AlliesHaveCapturedPOI()
    {
        for(int i = 1; i< FactionManager.Instance.factions.Count; i++)
        {
            if(FactionManager.Instance.GetReputation(_unit.targetPOI.ownerFaction, _unitBT.faction) > 0 && _unit.targetPOI.capturePercentage == 100)
            {
                return true;
            }
        }
        return false;
    }
}
