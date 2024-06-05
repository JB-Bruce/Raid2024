using UnityEngine;
using static Node_script;
public class IsPOICaptured : Node
{
    UnitMovement _unit;
    Humanoid _humanoid;
    UnitBT _unitBT;
    FactionManager _factionManager;
    CircleCollider2D _POIDetection;

    public IsPOICaptured(UnitMovement unit)
    {
        _factionManager = FactionManager.Instance;
        _unit = unit;
        _humanoid = unit.gameObject.GetComponent<Humanoid>();
        _unitBT = unit.gameObject.GetComponent<UnitBT>();
        _POIDetection = _unit.gameObject.GetComponent<CircleCollider2D>();
    }

    public override NodeState Evaluate()
    {
        if(_unit.targetPOI.ownerFaction == _humanoid.faction && _unit.targetPOI.capturePercentage >= 100)
        {
            if(!_unit.poiHasBeenCaptured)
            {
                _unit.SetGuardPoint(_unit.targetPOI.poiPosition.position, _unit.targetPOI.minDistancePOI, _unit.targetPOI.maxDistancePOI);
                _unit.ChangeTarget(_unit.GetRandomPointOnGuardPoint());
            }
            _unit.poiHasBeenCaptured = true;

            return NodeState.FAILURE;
        }

        if(AlliesHaveCapturedPOI())
        {
            _unitBT.order = UnitOrder.Patrol;
            for (int i = 0; i < _factionManager.factions.Count; i++)
            {
                if (_factionManager.factions[i].FactionUnitManager.faction == _unitBT.faction)
                {
                    _factionManager.factions[i].FactionUnitManager.RemoveJob(_unit.gameObject);
                }
            }
            _unit.ChangeTarget(_unit.GetRandomPointOnMap());
        }

        if(_unit.poiHasBeenCaptured)
        {
            _unit.poiHasBeenCaptured = false;
            _unit.SetGuardPoint(_unit.targetPOI.transform.position, 0, _POIDetection.radius);
            _unit.SetGuardPoint(_unit.GetRandomPointOnGuardPoint(), 0, 0);
        }
            return NodeState.SUCCESS;
    }

    // Return true if an ally captured the POI
    private bool AlliesHaveCapturedPOI()
    {
        for(int i = 1; i< _factionManager.factions.Count; i++)
        {
            if(_factionManager.GetReputation(_unit.targetPOI.ownerFaction, _unitBT.faction) > 0 && _unit.targetPOI.capturePercentage >= 100)
            {
                return true;
            }
        }
        return false;
    }
}
