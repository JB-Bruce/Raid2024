using System;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;
    public List<FactionUnitManager> factions = new List<FactionUnitManager>();
    public List<Reputation> reputations = new List<Reputation>();
    public List<POI> poi = new List<POI>();
    public float maxReputation = 8;
    public float minReputation = -5;
    public float neutralReputation = -1;
    public float allyReputation = 1;
    public float changeReputationForAllies = 0.1f;
    public Vector2[] vertices = new Vector2[4];  // Les sommets du losange

    private static readonly List<Faction> _interactibleFaction = new List<Faction>() { Faction.Player, Faction.Survivalist, Faction.Scientist, Faction.Military, Faction.Utopist};


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        poi = OrderPOIByPriority();
    }

    // Remove a unit from a faction and indicate is death to the faction
    public void RemoveUnitFromFaction(Faction faction, GameObject unit)
    {
        for(int i = 0; i < factions.Count; i++) 
        {
            if(factions[i].faction == faction)
            {
                factions[i].RemoveJob(unit);
                factions[i].nbrOfDeadUnit++;
                factions[i].units.Remove(unit);
            }
        }
    }

    // Add the newReputation to the reputation between factions

    public void AddReputation(Faction faction1, Faction faction2, float newReputation)
    {
        for(int i = 0;i < reputations.Count;i++)
        {
            if ((reputations[i].faction1 == faction1 || reputations[i].faction2 == faction1) &&
                (reputations[i].faction1 == faction2 || reputations[i].faction2 == faction2) && 
                faction1 != faction2) 
            {
                Reputation reputation = reputations[i];
                reputation.reputation += newReputation;
                reputations[i] = reputation;
                return;
            }
        }
    }

    // Return the reputation between faction
    public float GetReputation(Faction faction1, Faction faction2) 
    {
        if(faction1 == Faction.Bandit || faction2 == Faction.Bandit)
        {
            return -5;
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
        if (kill == Faction.Bandit)
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
        return IsPointInTriangle(point, vertices[0], vertices[1], vertices[2]) ||
               IsPointInTriangle(point, vertices[0], vertices[2], vertices[3]);
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

    //// Get a random point in a rhombus
    //public Vector3 GetRandomPointInRhombus()
    //{
    //    // Calculate half-diagonals
    //    float halfDiagonal1 = Vector2.Distance(vertices[0], vertices[2]) / 2f;
    //    float halfDiagonal2 = Vector2.Distance(vertices[1], vertices[3]) / 2f;

    //    // Generate random coordinates within the rhombus
    //    float randomX = UnityEngine.Random.Range(-halfDiagonal1, halfDiagonal1);
    //    float randomZ = UnityEngine.Random.Range(-halfDiagonal2, halfDiagonal2);

    //    // Transform local coordinates to world coordinates
    //    Vector3 randomPoint = new Vector3(randomX, randomZ);

    //    return randomPoint;
    //}

}


// Contain the reference of the reputation of one faction to another
[System.Serializable]
public struct Reputation
{
    public Faction faction1;
    public Faction faction2;
    [Range(-5,8)] public float reputation;
}


