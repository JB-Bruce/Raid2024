using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField]
    private List<Humanoid> humanoidAround = new();
    Transform _transform;

    [Header("Attack Parameters")]
    public bool canAttack = false;
    public Vector3 lastPosition = Vector3.zero;
    public GameObject lastEnemy;
    public float viewRange = 31f;

    public Humanoid nearestEnemy;
    private Humanoid _mHumanoid;

    public Weapon weapon;
    private WeaponAttack weaponAttack;

    [Header("Update Parameters")]
    private int evaluateUpdate = 0;
    public int jumpUpdate = 2;

    [Header("Reputation")]
    public float neutralReputation = 0;

    [Header("Detection")]
    public CircleCollider2D circleCollider;

    public void Init()
    {
        weaponAttack = GetComponent<WeaponAttack>();
        _transform = transform;
        circleCollider.radius = viewRange;
        _mHumanoid = GetComponent<Humanoid>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Humanoid>(out Humanoid humanoid)&& !humanoidAround.Contains(humanoid) && humanoid.faction != _mHumanoid.faction)
        {
            humanoidAround.Add(humanoid);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoidAround.Remove(humanoid);
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
            if (humanoidAround[i] != null && humanoidAround[i].faction != _mHumanoid.faction && FactionManager.Instance.GetReputation(humanoidAround[i].faction, _mHumanoid.faction) < neutralReputation)
            {
                ennemies.Add(humanoidAround[i]);
            }
            else if(humanoidAround[i] == null)
            {
                humanoidAround.RemoveAt(i);
                i--;
            }
        }
        return ennemies;
    }

    // Get the nearrest enemy
    public Humanoid GetNearrestEnemy()
    {
        int nearest = 0;
        float _distanceToNearest = Mathf.Infinity;
        bool _nearestFound = false;

        List<Humanoid> ennemies = EnnemiesAround();

        for (int i = 0; i < ennemies.Count; i++) 
        {
            float _newDistance = Vector3.Distance(_transform.position, ennemies[i].transform.position);
            if(_distanceToNearest >= _newDistance)
            {
                RaycastHit2D _hit = Physics2D.Raycast(_transform.position, ennemies[i].transform.position - _transform.position, viewRange);

                if (_hit.collider != null && _hit.collider.gameObject == ennemies[i].gameObject)
                {
                    nearest = i;
                    _distanceToNearest = _newDistance;
                    _nearestFound = true;
                    lastEnemy = ennemies[i].gameObject;
                }
            }
               
        }

        if(_nearestFound) 
        {
            return ennemies[nearest];
        }
        return null;
    }

    private void Update()
    {
        evaluateUpdate++;

        if(evaluateUpdate > jumpUpdate) 
        {
            evaluateUpdate = 0;
            nearestEnemy = GetNearrestEnemy();
            if (nearestEnemy != null)
            {
                lastPosition = nearestEnemy.transform.position;
            }
        }

        if(canAttack && nearestEnemy != null) 
        {
            weaponAttack.EquipWeapon(this);
            weaponAttack.UseWeapon(nearestEnemy.transform.position - _transform.position);

                //if(nearestEnemy.TakeDamage(20))
                //{
                //    nearestEnemy = null;
                //    lastPosition = Vector3.zero;
                //}
        }
    }
}
