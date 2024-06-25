using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : FactionBuilding
{
    private FactionUnitManager _factionUnit;

    protected override void UpgradeEffect()
    {
        if(_factionUnit == null)
        {
            _factionUnit = FactionManager.Instance.GetFaction(faction).FactionUnitManager;
        }
        _factionUnit.drawWeapons = _factionUnit.WeaponSpawnPerLevel[_buildingLevel - 2]._drawWeapons;
    }
}

[Serializable]
public struct WeaponsDrawLevel
{
    public List<DrawWeapon> _drawWeapons;
}
