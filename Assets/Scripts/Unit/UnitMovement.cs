using UnityEngine;
using UnityEngine.AI;


public class UnitMovement : MonoBehaviour
{
    private float _mapSize;
    private NavMeshAgent _agent;
    private Transform _transform;

    private Vector3 _guardPoint;
    private float _minDistanceGuardPoint;
    private float _maxDistanceGuardPoint;
    public POI targetPOI;
    public bool poiHasBeenCaptured = false;

    private GameManager _gameManager;

    // Start is called before the first frame update
    public void Init()
    {
        _gameManager = GameManager.Instance;
        _mapSize = _gameManager.mapSize;
        _transform = transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

    }

    // Change the target point of the unit
    public void ChangeTarget(Vector3 target)
    {
         _agent.SetDestination(new Vector3(target.x, target.y, _transform.position.z));
    }

    // get random point on the map
    public Vector3 GetRandomPointOnMap()
    {
        Vector3 target;
        do
        {
            target = new Vector3(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize), _transform.position.z);
        } while (IsThePointRestricted(target));

        return target;
    }

    // Is the target point in the restricted area
    private bool IsThePointRestricted(Vector3 position)
    {
        for(int i = 0; i < _gameManager.restrictedAreas.Count; i++) 
        {
            if (Vector3.Distance(_gameManager.restrictedAreas[i].areaOrigine.position, position) <= _gameManager.restrictedAreas[i].areaRadius /*|| !NavMesh.SamplePosition(position, out NavMeshHit hit, 0.1f, 1)*/)
            {
                return true;
            }
        }
        return false;   
    }


    // Get a random point in the guard point of the unit
    public Vector3 GetRandomPointOnGuardPoint()
    {

            Vector3 randomDirection = Random.insideUnitCircle;

            float randomDistance = Random.Range(_minDistanceGuardPoint, _maxDistanceGuardPoint);

            return _guardPoint + randomDirection * randomDistance;
    }

    // set the guard area
    public void SetGuardPoint(Vector3 guardPoint, float minDistanceGuardPoint = 0, float maxDistanceGuardPoint = 0)
    {
         _guardPoint = guardPoint;
        _maxDistanceGuardPoint = maxDistanceGuardPoint;
        _minDistanceGuardPoint = minDistanceGuardPoint;
    }

    public Vector3 GetGuardPoint()
    {
        return _guardPoint;
    }

}
