using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;
    public List<FactionSc> factions = new List<FactionSc>();
    public List<Reputation> reputations = new List<Reputation>();
    public List<POI> poi = new List<POI>();
    public float maxReputation = 8;
    public float minReputation = -5;
    public float neutralReputation = -1;
    public float allyReputation = 1;
    public float changeReputationForAllies = 0.1f;
    public Transform[] vertices = new Transform[4];  // Les sommets du losange

    private StatsManager _statManager;

    public UnityEvent<POI, Faction> IsPoiCaught = new();
    public UnityEvent<Faction, float> RelationWithPlayerChange = new();

    [SerializeField] private List<FactionPlacement> _factionsPlacement = new List<FactionPlacement>();

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _textReputationPlayerScientist;
    [SerializeField] private TextMeshProUGUI _textReputationPlayerUtopist;
    [SerializeField] private TextMeshProUGUI _textReputationPlayerSurvivalist;
    [SerializeField] private TextMeshProUGUI _textReputationPlayerMilitary;

    [Header("Color")]
    [SerializeField] private Color _enemyColor;
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _allyColor;

    [Header("Sprites")]
    [SerializeField] private Sprite _enemySprite;
    [SerializeField] private Sprite _neutralSprite;
    [SerializeField] private Sprite _allySprite;

    public BonusFaction[] bonus = new BonusFaction[4];

    private static readonly List<Faction> _interactibleFaction = new List<Faction>() { Faction.Survivalist, Faction.Scientist, Faction.Military, Faction.Utopist};
    public List<FactionRespawn> factionRespawns = new List<FactionRespawn>();
    [SerializeField] private List<FactionMenu> _factionsMenus = new List<FactionMenu>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        poi = OrderPOIByPriority();
        SetFactionsPosition();
    }

    private void Start()
    {
        if (TutorialManager.Instance != null)
        {
            AddReputation(Faction.Player, TutorialManager.Instance.GetPlayerFaction(), 20);
        }
        for (int i = 0; i < factions.Count; i++) 
        {
            IsPoiCaught.AddListener(factions[i].HaveCaughtPOI);
        }
        SetReputationText();
    }

    // Remove a unit from a faction and indicate is death to the faction
    public void RemoveUnitFromFaction(Faction faction, GameObject unit)
    {
        for(int i = 0; i < factions.Count; i++) 
        {
            FactionUnitManager _factionUnit = factions[i].FactionUnitManager;
            if (_factionUnit.faction == faction)
            {
                _factionUnit.RemoveJob(unit);

                if(unit.GetComponent<Humanoid>().CanRespawn)
                {
                    _factionUnit.nbrOfDeadUnit++;
                }

                _factionUnit.units.Remove(unit);
            }
        }
    }

    // Add the newReputation to the reputation between factions

    public void AddReputation(Faction faction1, Faction faction2, float newReputation)
    {
        if(faction1 == Faction.Bandit ||  faction2 == Faction.Bandit || faction1 == Faction.Null || faction2 == Faction.Null)
        {
            return;
        }

        for(int i = 0;i < reputations.Count;i++)
        {
            if ((reputations[i].faction1 == faction1 || reputations[i].faction2 == faction1) &&
                (reputations[i].faction1 == faction2 || reputations[i].faction2 == faction2) && 
                faction1 != faction2) 
            {
                if(i < 4) 
                {
                    Faction changeRelation = faction1 == Faction.Player ? faction2 : faction1;

                    RelationWithPlayerChange.Invoke(changeRelation, newReputation);
                }

                Reputation reputation = reputations[i];
                reputation.reputation += newReputation;
                reputation.reputation = Mathf.Clamp(reputation.reputation, -5, 8);
                reputations[i] = reputation;
                ChangeReputationText(faction1, faction2, reputations[i].reputation);
                CanAlwaysRespawnInFaction(faction1, faction2, reputation.reputation);
                return;
            }
        }
    }


    // Check if the Player is not ennemy with the faction where he set is respawn
    private void CanAlwaysRespawnInFaction(Faction faction1, Faction faction2, float reputation)
    {
        if(_statManager == null)
        {
            _statManager = StatsManager.instance;
        }

        if((faction1 == Faction.Player || faction2 == Faction.Player) &&
            (_statManager.CastToEFactionRespawn(faction1) == _statManager.GetRespawnFaction() || _statManager.CastToEFactionRespawn(faction2) == _statManager.GetRespawnFaction()))
        {
            _statManager.ChangeRespawnFaction(StatsManager.ERespawnFaction.Null);
        }
    }

    // Set all the reputation Text
    private void SetReputationText()
    {
        for (int i = 0; i < reputations.Count; i++)
        {
            ChangeReputationText(reputations[i].faction1, reputations[i].faction2, reputations[i].reputation);
        }
    }

    // Return the reputation between faction
    public float GetReputation(Faction faction1, Faction faction2) 
    {

        if (faction1 == Faction.Bandit || faction2 == Faction.Bandit)
        {
            return -5;
        }

        if (faction1 == Faction.Null || faction2 == Faction.Null)
        {
            return 0;
        }

        for (int i = 0; i < reputations.Count; i++)
        {
            if (((reputations[i].faction1 == faction1 && reputations[i].faction2 == faction2) ||
                (reputations[i].faction1 == faction2 && reputations[i].faction2 == faction1)) &&
                faction1 != faction2)
            {
                return reputations[i].reputation;
            }
        }
        return -5;
    }

    // Order POI by priority
    private List<POI> OrderPOIByPriority()
    {
        int priority = 0;
        List<POI> orderList = new List<POI>();

        while (orderList.Count != poi.Count)
        {
            for (int i = 0; i < poi.Count; i++)
            {
                if (poi[i].priority == priority)
                {
                    orderList.Add(poi[i]);
                }
            }
            priority++;
        }
        return orderList;
    }

    public POI GetRandomPOI(FactionUnitManager faction)
    {
        List<POI> canTakePOI = new List<POI>();
        int priority = -1;

        for(int i = poi.Count-1 ; i >= 0 ; i--) 
        {
            if (GetReputation(faction.faction, poi[i].ownerFaction) < neutralReputation && poi[i].capturePercentage != 100 && (priority == -1 || priority == poi[i].priority) && !faction.isPOIFull(i) )
            {
                priority = poi[i].priority;
                canTakePOI.Add(poi[i]);

            }
        }
        if(priority != -1) 
        {
            return canTakePOI[UnityEngine.Random.Range(0, canTakePOI.Count)];
        }
        return null;
        
    }

    // Add Reputation to Allies and remove reputation to the allies of the kill unit's faction
    public void ChangeAllReputation(Faction killer, Faction kill)
    {
        if (kill == Faction.Bandit || killer == Faction.Bandit || kill == Faction.Null || killer == Faction.Null)
            return;
        for(int i = 0 ; i< _interactibleFaction.Count; i++) 
        {
                if (kill != _interactibleFaction[i])
            { 
                if(GetReputation(kill, _interactibleFaction[i]) >= allyReputation)
                {
                    AddReputation(killer, _interactibleFaction[i], -changeReputationForAllies);
                }
                else if (GetReputation(kill, _interactibleFaction[i]) < neutralReputation)
                {
                    AddReputation(killer, _interactibleFaction[i], changeReputationForAllies);
                }
            }
        }
    }

    //Is the point in Rhombus
    public bool IsPointInRhombus(Vector2 point)
    {
        return IsPointInTriangle(point, vertices[0].position, vertices[1].position, vertices[2].position) ||
               IsPointInTriangle(point, vertices[0].position, vertices[2].position, vertices[3].position);
    }

    // Is the Point in the triangle
    bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
        float d1 = Sign(point, a, b);
        float d2 = Sign(point, b, c);
        float d3 = Sign(point, c, a);

        bool has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }

    // Get the Sign of a triangle
    float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    // Change the Text of the reputation
    private void ChangeReputationText(Faction _faction1, Faction _faction2, float reputation)
    {
        if(_faction1 == Faction.Player || _faction2 == Faction.Player) 
        {
            Faction _otherFaction = _faction1 == Faction.Player ? _faction2 : _faction1;

            Sprite sprite = null;

            if (reputation < neutralReputation)
            {
                sprite = _enemySprite;
            }
            else if (reputation > allyReputation)
            {
                sprite = _allySprite;
            }
            else
            {
                sprite = _neutralSprite;
            }

            switch(_otherFaction) 
            {
                case Faction.Survivalist:
                    _textReputationPlayerSurvivalist.text = reputation.ToString("f1");
                    _textReputationPlayerSurvivalist.transform.parent.GetComponent<Image>().sprite = sprite;
                    PlayerReputationParameters(_textReputationPlayerSurvivalist, reputation, _otherFaction);
                    break;

                case Faction.Utopist:
                    _textReputationPlayerUtopist.text = reputation.ToString("f1");
                    _textReputationPlayerUtopist.transform.parent.GetComponent<Image>().sprite = sprite;
                    PlayerReputationParameters(_textReputationPlayerUtopist, reputation, _otherFaction);
                    break;

                case Faction.Scientist:
                    _textReputationPlayerScientist.text = reputation.ToString("f1");
                    _textReputationPlayerScientist.transform.parent.GetComponent<Image>().sprite = sprite;
                    PlayerReputationParameters(_textReputationPlayerScientist, reputation, _otherFaction);
                    break;

                case Faction.Military:
                    _textReputationPlayerMilitary.text = reputation.ToString("f1");
                    _textReputationPlayerMilitary.transform.parent.GetComponent<Image>().sprite = sprite;
                    PlayerReputationParameters(_textReputationPlayerMilitary, reputation, _otherFaction);
                    break;
            }
        }

    }

    // Set Reputation parameters with player
    private void PlayerReputationParameters(TextMeshProUGUI _text, float _reputation, Faction _faction) 
    {
        if(_reputation >= allyReputation)
        {
            _text.color = _allyColor;
        }
        else if( _reputation < allyReputation && _reputation >= neutralReputation) 
        {
            _text.color = _neutralColor;
        }
        else
        {
            _text.color = _enemyColor;
        }

        int _index = GetBonusFaction(_faction);

        BonusFaction _bonusTmp = bonus[_index];

        if(_reputation < 1)
        {
            _bonusTmp.bonus = -1;
        }
        else if(_reputation < 2.5)
        {
            _bonusTmp.bonus = 0;
        }
        else if(_reputation < 5)
        {
            _bonusTmp.bonus = 1;
        }
        else
        {
            _bonusTmp.bonus = 2;
        }
        bonus[_index] = _bonusTmp;

    }

    // return the BonusFaction index
    private int GetBonusFaction(Faction _faction)
    {
        for(int i = 0; i < bonus.Length; i++) 
        {
            if (bonus[i].faction == _faction)
            {
                return i;
            }
        }
        return -1;
    }

    // Return the FactionSc
    public FactionSc GetFaction(Faction _faction)
    {
        for (int i = 0; i < factions.Count; i++)
        {
            FactionUnitManager _factionUnit = factions[i].FactionUnitManager;

            if (_faction == factions[i].FactionUnitManager.faction)
            {
                return factions[i];
            }
        }
        return null;
    }

    //set the position of factions on a random point and all the buildings
    private void SetFactionsPosition()
    {
        for(int i = 1; i < factions.Count;i++) 
        {
            int _randomIndex = UnityEngine.Random.Range(0, _factionsPlacement.Count);
            factions[i].transform.position = _factionsPlacement[_randomIndex].position.position;
            factions[i].SetGuardPoint(_factionsPlacement[_randomIndex].guardPoint1.localPosition, _factionsPlacement[_randomIndex].guardPoint2.localPosition);
            factions[i].FactionLeader.buildings = _factionsPlacement[_randomIndex].buildings;
            for(int j = 0; j < _factionsPlacement[_randomIndex].buildings.Count; j++) 
            {
                _factionsPlacement[_randomIndex].buildings[j].faction = factions[i].FactionUnitManager.faction;
            }
            int index = GetFactionMenuIndex(factions[i].FactionUnitManager.faction);
            _factionsPlacement[_randomIndex].buildingTriggers.SetPNJ(_factionsMenus[index]);
            _factionsMenus[index].SetHighlight(_factionsPlacement[_randomIndex].highlight);
            _factionsPlacement.RemoveAt(_randomIndex);
        }
    }

    // return the index of the faction menu of the faction parameter
    private int GetFactionMenuIndex(Faction faction)
    {
        for(int i = 0; i < _factionsMenus.Count;i++)
        {
            if(_factionsMenus[i].GetFaction() == faction)
                return i;
        }
        return -1;
    }
}



// Contain the reference of the reputation of one faction to another
[System.Serializable]
public struct Reputation
{
    public Faction faction1;
    public Faction faction2;
    [Range(-5,8)] public float reputation;
}

//Contain the repoawn powition of a faction
[System.Serializable]
public struct FactionRespawn
{
    public Faction RespawnFaction;
    public Transform RespawnTransform;
}

//Contain the Malus/Bonus of the player by Faction with (-1 = malus, 0 = 0%, 1 = 50% and 2 = 100%)
[System.Serializable]
public struct BonusFaction
{
    public Faction faction;
    [Range(-1,2)] public int bonus;
}

// Contain all the information for place a faction at a new position
[System.Serializable]
public struct FactionPlacement
{
    public Transform position;
    public Transform guardPoint1;
    public Transform guardPoint2;
    public List<FactionBuilding> buildings;
    public BuildingsTriggers buildingTriggers;
    public GameObject highlight;
}
