using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FactionUnitManager : MonoBehaviour
{
    private int indexeur = 0;

    public Faction faction;
    public GameObject unit;
    private Transform _transform;
    public Transform parent;
    private float _mapSize;
    public float SpawnDistanceAroundPlayer = 50;
    public List<DrawUnit> drawUnits = new List<DrawUnit>();

    [Header("Unit Management")]
    public List<GameObject> units = new List<GameObject>();
    public List<SurveillancePoint> surveillancePoints = new();
    private int _numberOfGuard = 0;
    public int maxUnit = 10;
    public int maxGuard = 3;
    public int nbrOfDeadUnit = 0;
    public float _unitSpawnRate = 30f;

    [Header("Guard Point")]
    public Transform point;
    public float minDistance;
    public float maxDistance;

    [Header("POI")]
    public int maxUnitPerPOI = 5;
    public int maxUnitOnPOI = 10;
    private int _numberOfPOIUnit = 0;
    private List<int> _unitOnPOI = new List<int>();

    private GameManager _gameManager;
    private FactionManager _factionManager;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _factionManager = FactionManager.Instance;

        SetUnitOnPOI();
        _mapSize = _gameManager.mapSize;
        _transform = transform;
        for (int i = 0; i< maxUnit; i++) 
        {
            SpawnUnit();
        }
        StartCoroutine(CheckUnitRoutine());
    }

    // Coroutine for CheckUnit() fonction
    private IEnumerator CheckUnitRoutine()
    {
        while(true) 
        {
            CheckUnit();
            yield return new WaitForSeconds(1f);
        }

    }

    // Make spawn a unit
    private void SpawnUnit()
    {
        GameObject go = Instantiate<GameObject>(DrawUnit(), parent);

        indexeur++;
        go.name = faction + indexeur.ToString();

        UnitBT unitBT = go.GetComponent<UnitBT>();
        unitBT.Init();

        if(faction == Faction.Bandit) 
        {
            go.transform.position = GetRandomSpawnPoint();
        }
        else
            go.transform.position = _transform.position;
        
        units.Add(go);

        GiveAJob(unitBT, _transform.position);
        unitBT.faction = faction;
    }

    // Give a job to unit
    private void GiveAJob(UnitBT BT, Vector3 position)
    {
        UnitMovement movement = BT.gameObject.GetComponent<UnitMovement>();

        if (faction != Faction.Bandit)
        {

            for (int i = 0; i < surveillancePoints.Count; i++)
            {
                if (surveillancePoints[i].unit == null)
                {
                    BT.order = UnitOrder.Surveillance;

                    SurveillancePoint surveillancePoint = surveillancePoints[i];
                    surveillancePoint.unit = BT.gameObject;
                    surveillancePoints[i] = surveillancePoint;
                    movement.SetGuardPoint(surveillancePoints[i].point.position);
                    movement.ChangeTarget(surveillancePoints[i].point.position);

                    return;
                }
            }

            if (Random.Range(0, 100) < 50 && _numberOfGuard < maxGuard)
            {
                BT.order = UnitOrder.AreaGuard;
                movement.SetGuardPoint(point.position, minDistance, maxDistance);
                _numberOfGuard++;
                return;
            }
        }

        if(Random.Range(0, 100) < 50 && _numberOfPOIUnit < maxUnitOnPOI)
        {
            BT.order = UnitOrder.POICapture;

            int random = _factionManager.poi.IndexOf(_factionManager.GetRandomPOI(this));

            if (random > 0)
            {
                _numberOfPOIUnit++;

                movement.targetPOI = _factionManager.poi[random];
                _unitOnPOI[random]++;

                movement.SetGuardPoint(movement.targetPOI.transform.position, 0, movement.gameObject.GetComponent<CircleCollider2D>().radius);
                movement.SetGuardPoint(movement.GetRandomPointOnGuardPoint(), 0, 0);

                return;
            }
        }

        BT.order = UnitOrder.Patrol;

    }

    //See if there is enouth unit on the map and make spawn one.
    public void CheckUnit()
    {
        if(nbrOfDeadUnit > 0)
        {
            nbrOfDeadUnit--;
            StartCoroutine(SpawnUnitWithDelay(_unitSpawnRate));
        }
    }

    // Coroutine for Spawn a unit with delay
    private IEnumerator SpawnUnitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnUnit();
    }

    // get random point on the map
    public Vector3 GetRandomSpawnPoint()
    {
        Vector3 target;
        do
        {
            target = new Vector3(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize), _transform.position.z);
        } while (CantSpawnHere(target));

        return target;
    }

    // can the unit spawn here
    private bool CantSpawnHere(Vector3 position)
    {
        for (int i = 0; i < _gameManager.restrictedAreas.Count; i++)
        {
            if ((Vector3.Distance(_gameManager.restrictedAreas[i].areaOrigine.position, position) <= GameManager.Instance.restrictedAreas[i].areaRadius
                || /*Replace by the player position*/ Vector3.Distance(Vector3.zero, position) <= SpawnDistanceAroundPlayer) || !NavMesh.SamplePosition(position, out NavMeshHit hit, 0.1f, 1) )
            {
                return true;
            }
        }
        return false;
    }

    // Set List unitOnPOI
    private void SetUnitOnPOI()
    {
        for(int i = 0; _factionManager.poi.Count > i; i++) 
        {
            _unitOnPOI.Add(0);
        }
    }

    // When the unit die change variable job
    public void RemoveJob(GameObject unit)
    {
        switch(unit.GetComponent<UnitBT>().order)
        {
            case UnitOrder.AreaGuard: 
                _numberOfGuard--; 
                break;

            case UnitOrder.POICapture:
                _numberOfPOIUnit--;
                _unitOnPOI[_factionManager.poi.IndexOf(unit.GetComponent<UnitMovement>().targetPOI)]--;
                break;
        }
       
    }

    // is the poi full of unit
    public bool isPOIFull(int index)
    {
        if (_unitOnPOI[index] > maxUnitPerPOI)
        {
            return true;
        }
        return false;
    }

    // Draw a unit 
    private GameObject DrawUnit()
    {
        int total = 0;
        for (int i = 0; i < drawUnits.Count; i++)
        {
            total += drawUnits[i].percentage;
        }

        int random = UnityEngine.Random.Range(0, total+1);
        int unitDrop = 0;

        for (int i = 0; i < drawUnits.Count; i++)
        {
            unitDrop += drawUnits[i].percentage;

            if (random <= unitDrop)
            {
                return drawUnits[i].unit;
            }
        }
        return null;
    }

}

// point used by unit for the suveillance
[System.Serializable]
public struct SurveillancePoint
{
    public Transform point;
    public GameObject unit;
}

// Contain a unit and is drop percentage
[System.Serializable]
public struct DrawUnit
{
    public GameObject unit;
    public int percentage;
}
