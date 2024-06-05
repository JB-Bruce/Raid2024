using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafeteria : FactionBuilding
{
    public List<float> respawnSpeedPerLevel = new();
    protected override void UpgradeEffect()
    {
        FactionUnitManager _factionUnit = FactionManager.Instance.GetFaction(faction).FactionUnitManager;
        _factionUnit.unitSpawnRate = respawnSpeedPerLevel[_buildingLevel - 2];
    }
}
