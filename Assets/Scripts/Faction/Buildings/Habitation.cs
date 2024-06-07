using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitation : FactionBuilding
{
    public List<int> newUnitPerLevel = new List<int>();

    protected override void UpgradeEffect()
    {
            FactionUnitManager _factionUnit = FactionManager.Instance.GetFaction(faction).FactionUnitManager;
            _factionUnit.nbrOfDeadUnit += newUnitPerLevel[_buildingLevel - 2];
        
    }
}
