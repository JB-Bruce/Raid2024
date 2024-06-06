using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitLeader : MonoBehaviour
{
    [SerializeField] protected List<UnitMovement> followers = new List<UnitMovement>();
    public Formation formation;
    [SerializeField] private GameObject _weapon;
    private NavMeshAgent _navMesh;
    public GameObject debuger;

    [SerializeField] private float _leaderSpeedMultiplier = 0.8f;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _weapon = _transform.GetComponentInChildren<WeaponAttack>().gameObject;
        _navMesh = GetComponent<NavMeshAgent>();
        _navMesh.speed *= _leaderSpeedMultiplier;
        StartCoroutine(SetFormationPosition());
    }

    // Add follower
    public void AddFollower(GameObject follower)
    {
        UnitBT _unitBT = follower.GetComponent<UnitBT>();
        follower.GetComponent<UnitBT>().master = this;
        _unitBT.order = UnitOrder.Surveillance;
        UnitMovement _unit = follower.GetComponent<UnitMovement>();
        followers.Add(_unit);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && followers.Count < formation.position.Count) 
        {
            AddFollower(debuger);
        }
    }

    // Coroutine for set the destination of the followers
    public IEnumerator SetFormationPosition()
    {
        while (true)
        {
            for (int i = 0; i < followers.Count; i++)
            {
                if (followers[i] == null)
                {
                    followers.RemoveAt(i);
                    i--;
                    continue;
                }

                followers[i].SetGuardPoint(_transform.position + _weapon.transform.up * formation.position[i].x + _weapon.transform.right * formation.position[i].y);
                //followers[i].transform.position = _transform.position + _weapon.transform.up * formation.position[i].x + _weapon.transform.right * formation.position[i].y;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (followers[i] != null)
            {
                followers[i].GetComponent<UnitBT>().order = UnitOrder.Patrol;
            }
        }
    }


}

// Contain all the information of a formation
[Serializable]
public struct Formation
{
    public List<Vector2> position;
}