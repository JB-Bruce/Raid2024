using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    public static FactionManager Instance;
    public List<FactionUnitManager> factions = new List<FactionUnitManager>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
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
}
