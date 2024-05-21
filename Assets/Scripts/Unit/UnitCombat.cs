using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> humanoidAround = new List<GameObject>();
    Transform _transform;

    //Attack Parameters
    private float _chargeTimer = 0;
    private float _attackSpeed = 1;
    public bool canAttack = false;
    public float attackDistance = 20f;
    public Vector3 lastPosition = Vector3.zero;
    public float viewRange = 31f;

    private void Start()
    {
        _transform = transform;
        GetComponent<CircleCollider2D>().radius = viewRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoidAround.Add(collision.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoidAround.Remove(collision.gameObject);
        }
    }

    // Check if an enemy humanoid is in the unit area
    public bool IsEnemyInMyArea()
    {
        if(EnnemiesAround().Count > 0 || lastPosition != Vector3.zero)
        {
            return true;
        }
        return false;
    }


    // return all ennemie around
    private List<Humanoid> EnnemiesAround()
    {
        List<Humanoid > ennemies = new List<Humanoid>();

        for(int i = 0; i < humanoidAround.Count; i++)
        {
            if (humanoidAround[i].GetComponent<Humanoid>().faction != GetComponent<Humanoid>().faction)
            {
                ennemies.Add(humanoidAround[i].GetComponent<Humanoid>());
            }
        }
        return ennemies;
    }

    // Get the nearrest enemy
    public GameObject GetNearrestEnemy()
    {
        int nearest = 0;
        float _distanceToNearest = Mathf.Infinity;
        bool _nearestFound = false;

        List<Humanoid> ennemies = EnnemiesAround();

        for (int i = 0; i < ennemies.Count; i++) 
        {
            float _newDistance = Vector3.Distance(_transform.position, ennemies[i].transform.position);
            RaycastHit2D _hit =  Physics2D.Raycast(_transform.position, ennemies[nearest].transform.position - _transform.position, viewRange);

            Debug.DrawRay(_transform.position, (ennemies[nearest].transform.position -_transform.position ) * viewRange, Color.red);

            if (_hit.collider != null && _distanceToNearest > _newDistance && _hit.collider.gameObject == ennemies[nearest].gameObject)
            {
                nearest = i;
                _distanceToNearest = _newDistance;
                _nearestFound = true;
            }
        }

        if(_nearestFound) 
        {
            lastPosition = ennemies[nearest].transform.position;
            return ennemies[nearest].gameObject;
        }
        return null;
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
