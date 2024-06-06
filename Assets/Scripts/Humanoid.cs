using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public bool isPlayer = false;

    public float life = 100;
    public Faction faction;
    public bool isDead = false;

    
    public MovePlayer player;
    private float reduceDamage;


    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage)
    {
        
        life -= damage;

        if (isPlayer) 
        {
            //if is this the player, get the amount of reduceDamage depending on the armors, and change the damage inflicted.

            player = GameObject.FindWithTag("Player").GetComponent<MovePlayer>();
            reduceDamage = player.CheckArmor();
            if(reduceDamage != 0)
            {
                reduceDamage = reduceDamage * damage / 100;
                life += reduceDamage;
            }
            
            if(life <= 0)
            {
                life=0;
            }
            
            Debug.Log("" + life);
            StatsManager.instance.ChangeLifeColor();
        }

        if (life <= 0 && !isDead)
        {
            isDead = true;
            Death();
            return true;
        }
        return false;
    }

    private void Death()
    {
        if (!isPlayer)
        {
            FactionManager.Instance.RemoveUnitFromFaction(faction, this.gameObject);
            Destroy(this.gameObject);
        }

    }
}
