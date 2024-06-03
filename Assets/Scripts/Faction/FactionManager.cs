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

    public void AddReputation(Faction faction1, Faction faction2, int newReputation)
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
}


// Contain the reference of the reputation of one faction to another
[System.Serializable]
public struct Reputation
{
    public Faction faction1;
    public Faction faction2;
    [Range(-5,8)] public float reputation;
}


