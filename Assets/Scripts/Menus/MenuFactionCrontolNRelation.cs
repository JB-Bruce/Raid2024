using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StatsManager;

public class MenuFactionCrontolNRelation : MonoBehaviour
{
    private FactionManager _factionManager;

    [Header("FactionRelation")]
    [SerializeField] private Sprite[] _relationSprite = new Sprite[3]; // Ally = 0 / Neutral = 1 / Ennemy = 2
    [SerializeField] private List<RelationInMenu> _relationsInMenu = new List<RelationInMenu>();
    private static readonly List<Faction> _factions = new List<Faction>() { Faction.Utopist, Faction.Scientist, Faction.Military, Faction.Survivalist };

    [Header("POI")]
    [SerializeField] private List<TextMeshProUGUI> _PoiOwners = new List<TextMeshProUGUI>();

    // Open the panel and set all the properties 
    public void OpenMenu()
    {
        if(_factionManager == null)
        {
            _factionManager = FactionManager.Instance;
        }
        gameObject.SetActive(true);
        SetRelations();
        SetPoiMenu();
    }

    // Set the Relation in the menu
    private void SetRelations()
    {
        int index = 0;
        for (int i = 0; i < _factions.Count; i++)
        {
            for (int j = i + 1; j < _factions.Count; j++)
            {
                SetRelations(_factions[i].ToString(), _factions[j].ToString(), _factionManager.GetReputation(_factions[i], _factions[j]), index++);
            }
        }
    }

    // Get and Set the POI
    private void SetPoiMenu()
    {
        for(int i = 0; i< _PoiOwners.Count; i++)
        {
            _PoiOwners[i].text = _factionManager.poi[i].GetOwnerFaction().ToString();
        }
    }

    // Close the panel
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    //Set all the relation in the menue
    private void SetRelations(string faction1, string faction2, float reputation, int index)
    {
        _relationsInMenu[index].faction1.text = faction1;
        _relationsInMenu[index].faction2.text = faction2;
        _relationsInMenu[index].relationImage.sprite = SetRelationSprite(reputation);
    }

    // Set the relation Sprite
    private Sprite SetRelationSprite(float reputation)
    {
        if(reputation < _factionManager.neutralReputation )
        {
            return _relationSprite[2];
        }
        else if(reputation < _factionManager.allyReputation)
        {
            return _relationSprite[1];
        }
        else 
        {
            return _relationSprite[0]; 
        }
    }


    // Contain information on one Relation in the Menu
    [Serializable]
    public struct RelationInMenu
    {
        public TextMeshProUGUI faction1;
        public TextMeshProUGUI faction2;
        public Image relationImage;
    }

}
