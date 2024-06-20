using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FactionLeader : MonoBehaviour
{

    public List<FactionBuilding> buildings = new List<FactionBuilding>();
    private int _buildingExperience = 0;

    private bool _allBuildingLevelMax = false;

    private int _levelOfLeader = 1;

    [SerializeField]
    private int _upgradePrice = 1000;

    [SerializeField]
    private float _upgradeMultiplicator = 1.5f;

    public int buildingPriorityUpgrade = -1;

    public UnityEvent<int> haveUpgradeBuilding = new UnityEvent<int>();
    public UnityEvent haveGainXP = new UnityEvent();

    private void Start()
    {
        GainBuildingXP(0);
    }

    // Give building experience to a faction for the auto upgrade
    public void GainBuildingXP(int XP)
    {
        _buildingExperience += XP;
        haveGainXP.Invoke();

        if(!_allBuildingLevelMax)
        {
            CheckUpgradeBuildings();
        }
    }

    // Check if the faction can upgrade a building and upgrade a random building or the priority building if he is specified
    public void CheckUpgradeBuildings()
    {
        if(_buildingExperience >= _upgradePrice) 
        {
            bool haveUpgrade = false;
            int upgradeIndex = -1;

            if(buildingPriorityUpgrade != -1 && buildings[buildingPriorityUpgrade]._buildingLevel < buildings[buildingPriorityUpgrade]._maxLevel) 
            {
                buildings[buildingPriorityUpgrade].UpgradeBuilding();
                haveUpgrade = true;
                upgradeIndex = buildingPriorityUpgrade;
            }
            else
            {
                List<int> upgradableBuilding = new List<int>();
                for(int i = 0; i < buildings.Count; i++) 
                {
                    if (buildings[i]._buildingLevel < buildings[i]._maxLevel)
                    {
                        upgradableBuilding.Add(i);
                    }
                } 

                if(upgradableBuilding.Count == 0)
                {
                    _allBuildingLevelMax=true;
                }

                else 
                {
                    int randomIndex = Random.Range(0, upgradableBuilding.Count);
                    buildings[upgradableBuilding[randomIndex]].UpgradeBuilding();
                    haveUpgrade = true;
                    upgradeIndex=randomIndex;
                }
            }

            if(haveUpgrade) 
            {
                GainBuildingXP(-_upgradePrice);
                _upgradePrice = (int)(_upgradePrice * _upgradeMultiplicator);
                haveUpgradeBuilding.Invoke(upgradeIndex);
                _levelOfLeader++;
                GainBuildingXP(0);
            }
        }
    }

    // Get LVL of leader
    public int LevelOfLeader => _levelOfLeader;

    // Get Upgrade price
    public int UpgradePrice => _upgradePrice;

    // Get building XP
    public int BuildingXP
    {
        get { return _buildingExperience; }
        set { _buildingExperience += value; }
    }
}
