using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;
    public List<FactionUnitManager> factions = new List<FactionUnitManager>();
    public List<Reputation> reputations = new List<Reputation>();
    public float maxReputation = 8;
    public float minReputation = -5;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Remove a unit from a faction and indicate is death to the faction
    public void RemoveUnitFromFaction(Faction faction, GameObject unit)
    {
        for(int i = 0; i < factions.Count; i++) 
        {
            if(factions[i].faction == faction)
            {
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
        for (int i = 0; i < reputations.Count; i++)
        {
            if ((reputations[i].faction1 == faction1 || reputations[i].faction2 == faction1) &&
                (reputations[i].faction1 == faction2 || reputations[i].faction2 == faction2) &&
                faction1 != faction2)
            {
                return reputations[i].reputation;
            }
        }
        return 0;
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
