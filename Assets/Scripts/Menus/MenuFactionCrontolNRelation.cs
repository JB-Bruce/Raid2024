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
    [SerializeField] private List<Image> _relationsInMenu = new List<Image>();
    private static readonly List<Faction> _factions = new List<Faction>() { Faction.Military, Faction.Utopist, Faction.Scientist, Faction.Survivalist };

    [Header("POI")]
    [SerializeField] private List<Image> _PoiOwners = new List<Image>();
    [SerializeField] private List<Sprite> _ImageFaction = new List<Sprite>(); // Null = 0, Mili = 1, Uto = 2, Sci = 3, Survi = 4

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
            _PoiOwners[i].sprite = GetFactionImage(_factionManager.poi[i].GetOwnerFaction());
        }
    }

    // Get image by faction
    public Sprite GetFactionImage(Faction faction)
    {
        switch(faction) 
        {
            case Faction.Military: return _ImageFaction[1];
            case Faction.Utopist: return _ImageFaction[2];
            case Faction.Scientist: return _ImageFaction[3];
            case Faction.Survivalist: return _ImageFaction[4];
            default: return _ImageFaction[0];
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
        _relationsInMenu[index].sprite = SetRelationSprite(reputation);
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
}
