using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FactionMenu : Interactable
{
    [SerializeField] private FactionSc _faction;
    private Inventory _inventory;

    private FactionLeader _leader;

    private FactionManager _factionManager;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField]
    private Transform _parentBuildingInfo;

    [Header("Slider Building XP")]
    [SerializeField] private RectTransform _slider;
    [SerializeField] private TextMeshProUGUI _currentLVL;
    [SerializeField] private TextMeshProUGUI _nextLVL;
    [SerializeField] private TextMeshProUGUI _progress;
    [SerializeField] private Transform _pointerTransform;
    [SerializeField] private Transform _indicatorTransform;

    [Header("Respawn")]

    private StatsManager _statsManager;
    [SerializeField] StatsManager.ERespawnFaction _eRespawnFaction;
    [SerializeField] private Toggle _toggle;

    [Header("Unit Recruitement")]
    private int _numberOfUnit = 1;
    [SerializeField] TextMeshProUGUI _numberOfUnitText;
    [SerializeField] private List<GameObject> _units;
    [SerializeField] private UnitLeader _unitLeader;
    [SerializeField] private List<ItemRequire> _itemText;
    [SerializeField] private List<int> _nbrOfItems = new();
    [SerializeField] private List<Item> _items = new();
    [SerializeField] private Button _recruitBtn;
    private bool _canTrade = false;
    [SerializeField] private GameObject _highlightSprite;


private void Init()
    {
        _leader = _faction.FactionLeader;
        _leader.haveUpgradeBuilding.AddListener(BuildingUpgrade);
        _leader.haveGainXP.AddListener(SetBuildingXPSlider);
        _statsManager = StatsManager.instance;
        _statsManager.haveChangeSpawn.AddListener(RespawnHaveChange);
        _inventory = Inventory.Instance;
        _factionManager = FactionManager.Instance;

        for (int i = 0; i < _items.Count; i++)
        {
            _itemText[i].itemRequireImage.sprite = _items[i].ItemSprite;
        }
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
        buildingInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _leader.buildings[index].buildingUpgradeInfo;
        buildingInfo.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = (_leader.buildings[index]._buildingLevel).ToString();
        buildingInfo.transform.GetChild(0).gameObject.SetActive(false);
        buildingInfo.transform.GetChild(2).GetComponent<Image>().sprite = _leader.buildings[index].buildingImage;
    }

    // Reset all properties who change with the building upgrade
    private void BuildingUpgrade(int index)
    {
        _leader.buildingPriorityUpgrade = -1;
        SetBuilding(_parentBuildingInfo.GetChild(index).gameObject, index);
    }

    // Set the slider of building XP
    private void SetBuildingXPSlider()
    {
        _currentLVL.text = (_leader.LevelOfLeader).ToString();
        _nextLVL.text = ((_leader.LevelOfLeader + 1)).ToString();
        float _sliderSize = Mathf.Clamp((float)(_leader.BuildingXP) / ((float)_leader.UpgradePrice), 0, 1);
        _slider.localScale = new Vector3(_sliderSize, 1, 1);
        _progress.text = _leader.BuildingXP.ToString() + " / " + _leader.UpgradePrice.ToString();
        _indicatorTransform.position = _pointerTransform.position;
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
            _parentBuildingInfo.GetChild(index).GetChild(0).gameObject.SetActive(false);
            _leader.buildingPriorityUpgrade = -1;
            return;
        }

        _parentBuildingInfo.GetChild(index).GetChild(0).gameObject.SetActive(true);
        if(_leader.buildingPriorityUpgrade != -1)
        {
            _parentBuildingInfo.GetChild(_leader.buildingPriorityUpgrade).GetChild(0).gameObject.SetActive(false);
        }
        _leader.buildingPriorityUpgrade = index;

    }

    // Set select building
    private void SetSelectBuilding()
    {
        for(int i = 0; i< _leader.buildings.Count; i++)
        {
            if (_leader.buildingPriorityUpgrade == i)
            {
                _parentBuildingInfo.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                _parentBuildingInfo.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
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
        if(_factionManager.GetReputation(Faction.Player, _faction.FactionUnitManager.faction) < _factionManager.neutralReputation)
        {
            _toggle.SetIsOnWithoutNotify(false);
            _toggle.interactable = false;
            return;
        }
        _toggle.interactable = true;
        if(_eRespawnFaction != _statsManager.GetRespawnFaction()) 
        {
            _toggle.SetIsOnWithoutNotify(false);
        }
        else
        {
            _toggle.SetIsOnWithoutNotify(true);
        }
    }

    // Set unit recruitement    
    public void UnitRecruitement(int addUnit)
    {
        _numberOfUnit += addUnit;

        if(_numberOfUnit < 1)
            _numberOfUnit = 3;
        if(_numberOfUnit > 3)
            _numberOfUnit = 1;

        _numberOfUnitText.text = _numberOfUnit.ToString();

        for(int i =0; i< _itemText.Count; i++)
        {
            _itemText[i].quantityRequire.text = (_nbrOfItems[i] * _numberOfUnit).ToString();
        }

        for (int i = 0; i < _units.Count; i++) 
        {
            if(i < _numberOfUnit)
                _units[i].SetActive(true);
            else
                _units[i].SetActive(false);
        }
        SetTradeButton();
    }

    // Recruit the unit (Call by the recruit button)
    public void Recruit()
    {
        _unitLeader.DisbandFormation();
        for(int i = 0; i< _numberOfUnit; i++) 
        {
            GameObject unit = _faction.FactionUnitManager.SpawnWaveUnit(_faction.gameObject.transform.position, Vector3.zero);
            _unitLeader.AddFollower(unit);

            for(int j = 0; j< _items.Count; j++ )
            {
                _inventory.RemoveItems(_items[j], _nbrOfItems[j]);
            }
        }
        SetTradeButton();
    }

    // Set the button interactible if we can trade item
    private void SetTradeButton()
    {
        CanTrade();
        if(_canTrade) 
        {
            _recruitBtn.interactable = true;
        }
        else
        {
            _recruitBtn.interactable = false;
        }
    }

    // Check if the trade can be made
    public void CanTrade()
    {
        _canTrade = true;
        for (int i = 0; i < _items.Count; i++)
        {
            int quantityInInventory = _inventory.CountItemInInventory(_items[i]);
            _itemText[i].quantityInInventory.text = quantityInInventory.ToString();
            if (quantityInInventory < (_nbrOfItems[i] * _numberOfUnit))
            {
                _canTrade = false;
            }
        }
    }

    // Open the panel and set all the properties 
    public void OpenFactionMenu()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);

        if (_statsManager == null)
        {
            Init();
        }

        _eRespawnFaction = _statsManager.CastToEFactionRespawn(_faction.FactionUnitManager.faction);

        _playerInput.SwitchCurrentActionMap("FactionMenu");
        
        SpawnBuildings();
        UnitRecruitement(0);
        RespawnHaveChange();
        SetBuildingXPSlider();
        SetSelectBuilding();
        SetTradeButton();
    }

    // Close the panel
    public void Close()
    {
        this.gameObject.SetActive(false);
        _playerInput.SwitchCurrentActionMap("InGame");
        Time.timeScale = 1.0f;
    }

    //Get Faction
    public Faction GetFaction()
    {
        return _faction.FactionUnitManager.faction;
    }

    // Set highlight sprite
    public void SetHighlight(GameObject highlight)
    {
        _highlightSprite = highlight;
    }

    // Gestion of Interaction
    protected override void Interact()
    {
        OpenFactionMenu();
    }

    public override void Highlight(bool state)
    {
        if (_highlightSprite != null)
        {
            _highlightSprite.SetActive(state);
        }
    }

    public override void TriggerEnter(bool state)
    {
        if (state)
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
        else
        {
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }

}

//public enum RomanNumeral
//{
//    I = 1,
//    II = 2,
//    III = 3,
//    IV = 4,
//    V = 5,
//    VI = 6,
//    VII = 7,
//    VIII = 8,
//    IX = 9,
//    X = 10,
//    XI = 11,
//    XII = 12,
//    XIII = 13,
//}
