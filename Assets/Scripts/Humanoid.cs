using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    public int life = 100;
    public Faction faction;

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
            Destroy(this.gameObject);
    }

}
