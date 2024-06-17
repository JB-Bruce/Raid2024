using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    [SerializeField]
    private List<Humanoid> humanoidAround = new();
    Transform _transform;
    private FactionManager _factionManager;

    [Header("Attack Parameters")]
    public bool canAttack = false;
    public Vector3 lastPosition = Vector3.zero;
    public GameObject lastEnemy;
    public float viewRange = 31f;

    public Humanoid nearestEnemy;
    private Humanoid _mHumanoid;

    public Weapon weapon;
    [HideInInspector]
    public WeaponAttack weaponAttack;

    [Header("Reputation")]
    public float neutralReputation = -1;

    [Header("Detection")]
    public CircleCollider2D circleCollider;

    public void Init()
    {
        _factionManager = FactionManager.Instance;
        weaponAttack = GetComponentInChildren<WeaponAttack>();

        weaponAttack.weaponChange.AddListener(ChangeWeapon);
        _transform = transform;
        circleCollider.radius = viewRange;
        _mHumanoid = GetComponent<Humanoid>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent == null)
            return;
        if (collision.transform.parent.parent == null)
            return;
        if (collision.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid)&& !humanoidAround.Contains(humanoid) && humanoid.faction != _mHumanoid.faction  && !collision.isTrigger)
        {
            humanoidAround.Add(humanoid);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent == null)
            return;
        if (collision.transform.parent.parent == null)
            return;
        if (collision.transform.parent.parent.TryGetComponent<Humanoid>(out Humanoid humanoid) && !collision.isTrigger)
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
            if (humanoidAround[i] != null && humanoidAround[i].faction != _mHumanoid.faction && _factionManager.GetReputation(humanoidAround[i].faction, _mHumanoid.faction) < neutralReputation)
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


                if (_hit.collider != null && _hit.collider.transform.parent.parent.gameObject == ennemies[i].gameObject && !_hit.collider.isTrigger)
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
            lastPosition = ennemies[nearest].transform.position;
            return ennemies[nearest];
        }
        return null;
    }

    // change the weapon
    private void ChangeWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

    private void Update()
    {
        if(canAttack && nearestEnemy != null) 
        {
            weaponAttack.UseWeapon(nearestEnemy.transform.position - weaponAttack.firePoint.transform.position, _mHumanoid.faction);
        }
        //weaponAttack.UpdateWeaponRotation();
    }

    // Get the faction of the owner unit
    public Faction GetFaction() 
    {
        return _mHumanoid.faction;
    }
}
