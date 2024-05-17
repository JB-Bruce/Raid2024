using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{
    public int mapSize;
    NavMeshAgent _agent;

    private Vector2 _guardPoint;
    private float _minDistanceGuardPoint;
    private float _maxDistanceGuardPoint;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Change the target point of the unit
    public void ChangeTarget(Vector3 target)
    {
         _agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    // get random point on the map
    public Vector3 GetRandomPointOnMap()
    {
        Vector3 target;
        do
        {
            target = new Vector3(Random.Range(-mapSize, mapSize), Random.Range(-mapSize, mapSize), transform.position.z);
        } while (IsThePointRestricted(target));

        return target;
    }

    // Is the target point in the restricted area
    private bool IsThePointRestricted(Vector3 position)
    {
        for(int i = 0; i < GameManager.Instance.restrictedAreas.Count; i++) 
        {
            if (Vector3.Distance(GameManager.Instance.restrictedAreas[i].areaOrigine, position) <= GameManager.Instance.restrictedAreas[i].areaRadius)
            {
                return true;
            }
        }
        return false;   
    }


    // Get a random point in the guard point of the unit
    public Vector3 GetRandomPointOnGuardPoint()
    {

            Vector2 randomDirection = Random.insideUnitCircle;

            float randomDistance = Random.Range(_minDistanceGuardPoint, _maxDistanceGuardPoint);

            return _guardPoint + randomDirection * randomDistance;
    }

    // set the guard area
    public void SetGuardPoint(Vector3 guardPoint, float minDistanceGuardPoint, float maxDistanceGuardPoint)
    {
         _guardPoint = guardPoint;
        _maxDistanceGuardPoint = maxDistanceGuardPoint;
        _minDistanceGuardPoint = minDistanceGuardPoint;
    }



}
