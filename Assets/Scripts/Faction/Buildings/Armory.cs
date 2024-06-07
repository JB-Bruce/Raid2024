using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : FactionBuilding
{
    [Serializable]
    public struct WeaponsDrawLevel
    {
        public List<DrawWeapon> _drawWeapons;
    }

    public List<WeaponsDrawLevel> WeaponSpawnPerLevel = new();

    protected override void UpgradeEffect()
    {
        FactionUnitManager _factionUnit = FactionManager.Instance.GetFaction(faction).FactionUnitManager;
        _factionUnit.drawWeapons = WeaponSpawnPerLevel[_buildingLevel - 2]._drawWeapons;
    }
}
