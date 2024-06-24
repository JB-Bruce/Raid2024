using UnityEngine;

public class ConstructionToDefend : Humanoid
{
    [SerializeField]
    private StatsManager _statsManager;

    protected override void Death(Faction _faction)
    {
        _statsManager.Death();
    }
}
