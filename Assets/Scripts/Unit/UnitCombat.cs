using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    private List<GameObject> humanoidInMyArea = new List<GameObject>();
    Transform _transform;

    //Attack Parameters
    private float _chargeTimer = 0;
    private float _attackSpeed = 1;
    public bool canAttack = false;

    public float attackDistance = 20f;

    private void Start()
    {
        _transform = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoidInMyArea.Add(collision.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoidInMyArea.Remove(collision.gameObject);
        }
    }

    // Check if an enemy humanoid is in the unit area
    public bool IsEnemyInMyArea()
    {
        if(humanoidInMyArea.Count > 0) 
            return true;
        return false;
    }

    // Get the nearrest enemy
    public GameObject GetNearrestEnemy()
    {
        int nerrest = 0;

        for (int i = 0; i < humanoidInMyArea.Count; i++) 
        {
            if (Vector3.Distance(_transform.position, humanoidInMyArea[nerrest].transform.position) > Vector3.Distance(_transform.position, humanoidInMyArea[i].transform.position))
            {
                nerrest = i;
            }
        }

        return humanoidInMyArea[nerrest];
    }

    private void Update()
    {
        if(canAttack && _chargeTimer < Time.time) 
        {
            _chargeTimer = Time.time + _attackSpeed;
            GetNearrestEnemy().GetComponent<Humanoid>().TakeDamage(20);
        }
    }
}
