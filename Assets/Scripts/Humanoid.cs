using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public bool isPlayer = false;

    public float life = 100;
    public Faction faction;
    public bool isDead = false;
    public float removeDeathReputation = -0.5f;
    public float removeHitReputation = -1;


    private FactionManager _factionManager;

    private void Awake()
    {
        _factionManager = FactionManager.Instance;
    }

    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage, Faction _faction)
    {
        life -= damage;
        _factionManager.AddReputation(faction, _faction, removeHitReputation);

        if (isPlayer) 
        {
            StatsManager.instance.ChangeLifeColor();
        }

        if (life <= 0 && !isDead)
        {
            isDead = true;
            Death(_faction);
        }
        return isDead;
    }

    // When a unit Die
    private void Death(Faction _faction)
    {
        if (!isPlayer)
        {
            _factionManager.RemoveUnitFromFaction(faction, this.gameObject);
            _factionManager.AddReputation(faction, _faction, removeDeathReputation);
            _factionManager.ChangeAllReputation(_faction, faction);
            Destroy(this.gameObject);
        }

    }
}
