using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionMenu : MonoBehaviour
{
    [SerializeField]
    private FactionSc _faction;

    private FactionLeader _leader;

    [SerializeField]
    private Transform _parentBuildingInfo;

    [Header("Slider Building XP")]
    [SerializeField] private RectTransform _slider;
    [SerializeField] private TextMeshProUGUI _currentLVL;
    [SerializeField] private TextMeshProUGUI _nextLVL;
    [SerializeField] private TextMeshProUGUI _progress;

    [Header("Respawn")]

    private StatsManager _statsManager;
    [SerializeField] StatsManager.ERespawnFaction _eRespawnFaction;
    [SerializeField] private Toggle _toggle;




    private void Start()
    {
        _leader = _faction.FactionLeader;
        SpawnBuildings();
        _leader.haveUpgradeBuilding.AddListener(BuildingUpgrade);
        _leader.haveGainXP.AddListener(SetBuildingXPSlider);
        _statsManager = StatsManager.instance;
        _statsManager.haveChangeSpawn.AddListener(RespawnHaveChange);
    }

    // Spawn All building Info
    private void SpawnBuildings()
    {
        for(int i=0; i<_leader.buildings.Count; i++) 
        {
            SetBuilding(_parentBuildingInfo.GetChild(i).gameObject, i);
        }
    }

    // Set a building Info
    private void SetBuilding(GameObject buildingInfo, int index)
    {
        buildingInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _leader.buildings[index].buildingDescription;
        buildingInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _leader.buildings[index].buildingUpgradeInfo;
        buildingInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _leader.buildings[index]._buildingLevel.ToString();
        buildingInfo.transform.GetChild(3).gameObject.SetActive(false);
        buildingInfo.transform.GetChild(4).GetComponent<Image>().sprite = _leader.buildings[index].buildingImage;
    }

    // Reset all properties who change with the building upgrade
    private void BuildingUpgrade(int index)
    {
        SetBuilding(_parentBuildingInfo.GetChild(index).gameObject, index);
    }

    // Set the slider of building XP
    private void SetBuildingXPSlider()
    {
        _currentLVL.text = _leader.LevelOfLeader.ToString();
        _nextLVL.text = (_leader.LevelOfLeader + 1).ToString();
        float _sliderSize = Mathf.Clamp((float)(_leader.BuildingXP) / ((float)_leader.UpgradePrice), 0, 1);
        _slider.localScale = new Vector3(_sliderSize, 1, 1);
        _progress.text = _leader.BuildingXP.ToString() + " / " + _leader.UpgradePrice.ToString();
    }

    // Select building the player want to upgrade in priority
    public void SelectBuilding(int index)
    {
        if(_leader.buildings[index]._buildingLevel >= _leader.buildings[index]._maxLevel) 
        {
            return;
        }

        if (_leader.buildingPriorityUpgrade == index )
        {
            _parentBuildingInfo.GetChild(index).GetChild(3).gameObject.SetActive(false);
            _leader.buildingPriorityUpgrade = -1;
            return;
        }

        _parentBuildingInfo.GetChild(index).GetChild(3).gameObject.SetActive(true);
        if(_leader.buildingPriorityUpgrade != -1)
        {
            _parentBuildingInfo.GetChild(_leader.buildingPriorityUpgrade).GetChild(3).gameObject.SetActive(false);
        }
        _leader.buildingPriorityUpgrade = index;

    }

    // Call whan the player clic on the check Box
    public void SpawnHere()
    {
        if(_toggle.isOn)
        {
            _statsManager.ChangeRespawnFaction(_eRespawnFaction);
        }
        else
        {
            _statsManager.ChangeRespawnFaction(StatsManager.ERespawnFaction.Null);
        }

    }

    // Call when the respawn point have change for uncheck the check box of other faction
    private void RespawnHaveChange()
    {
        if(_eRespawnFaction != _statsManager.GetRespawnFaction()) 
        {
            _toggle.SetIsOnWithoutNotify(false);
        }
    }

}
