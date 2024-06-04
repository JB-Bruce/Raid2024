using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public bool isPlayer = false;

    public float life = 100;
    public Faction faction;
    public bool isDead = false;
    public float removeDeathReputation = -0.5f;
    public float removeHitReputation = -1;

    public ParticleSystem pSystem;


    private FactionManager _factionManager;

    private void Awake()
    {
        _factionManager = FactionManager.Instance;
    }

    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage, Faction _faction, Vector2 fwd)
    {
        life -= damage;
        _factionManager.AddReputation(faction, _faction, removeHitReputation);

        pSystem.Play();

        if (isPlayer) 
        {
            StatsManager.instance.ChangeLifeColor();
        }

        if (life <= 0 && !isDead)
        {
            isDead = true;
            Death(_faction);
        }
        GetComponent<Rigidbody2D>().AddForce(fwd * 10, ForceMode2D.Impulse);
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

        if(_faction == Faction.Player)
        {
            QuestManager.instance.CheckQuestKill(faction);
        }
    }
}
