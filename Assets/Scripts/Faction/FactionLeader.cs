using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionLeader : MonoBehaviour
{

    public List<FactionBuilding> buildings = new List<FactionBuilding>();
    private int _buildingExperience = 0;

    // Give building experience to a faction for the auto upgrade
    public void GainBuildingXP(int XP)
    {
        _buildingExperience += XP;
        CheckUpgradeBuildings();
    }

    // Check if the faction can upgrade a building
    public void CheckUpgradeBuildings()
    {
        for (int i = 0; i < buildings.Count; i++) 
        {
            if(buildings[i].CanUpgradeBuilding(ref _buildingExperience))
            {
                return;
            }
        }
    }
}
