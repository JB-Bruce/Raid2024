using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactionBuilding : MonoBehaviour
{
    public Faction faction;
    protected int _buildingLevel = 1;
    [SerializeField] protected List<int> _upgradePrice = new List<int>();

    // Check if the building can be upgrade
    public bool CanUpgradeBuilding(ref int XP)
    {
        if (_buildingLevel  <= _upgradePrice.Count && _upgradePrice[_buildingLevel - 1] <= XP)
        {
            UpgradeBuilding(ref XP);
            return true; 
        }
        return false;
    }

    // Upgrade the building
    private void UpgradeBuilding(ref int XP)
    {
        XP -= _upgradePrice[_buildingLevel - 1];
        _buildingLevel++;
        UpgradeEffect();
    }

    // Override this fonction for make the upgrade effect of the building
    protected abstract void UpgradeEffect();

}
