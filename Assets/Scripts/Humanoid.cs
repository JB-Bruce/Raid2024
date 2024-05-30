using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public float life = 100;
    public Faction faction;
    public bool isDead = false;

    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage)
    {
        life -= damage;
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
        FactionManager.Instance.RemoveUnitFromFaction(faction, this.gameObject);
        Destroy(this.gameObject);

    }

}
