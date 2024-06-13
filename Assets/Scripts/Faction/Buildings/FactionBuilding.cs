using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class FactionBuilding : MonoBehaviour
{
    public Faction faction;
    public int _buildingLevel = 1;
    public int _maxLevel = 2;
    public Sprite buildingImage;
    public string buildingDescription;
    public string buildingUpgradeInfo;

    // Upgrade the building
    public void UpgradeBuilding()
    {
        _buildingLevel++;
        UpgradeEffect();
    }

    // Override this fonction for make the upgrade effect of the building
    protected abstract void UpgradeEffect();

}
