using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionUnitManager : MonoBehaviour
{
    public Faction faction;
    public GameObject unit;
    private Transform _transform;
    public Transform parent;
    private float _mapSize;
    public float SpawnDistanceAroundPlayer = 50;

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




    // Start is called before the first frame update
    void Start()
    {
        SetUnitOnPOI();
        _mapSize = GameManager.Instance.mapSize;
        _transform = transform;
        for (int i = 0; i< maxUnit; i++) 
        {
            SpawnUnit();
        }
        InvokeRepeating("CheckUnit", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Make spawn a unit
    private void SpawnUnit()
    {
        GameObject go = Instantiate<GameObject>(unit, parent);
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

            int random = FactionManager.Instance.poi.IndexOf(FactionManager.Instance.GetRandomPOI(this));

            if (random > 0)
            {
                _numberOfPOIUnit++;

                movement.targetPOI = FactionManager.Instance.poi[random];
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
            Invoke("SpawnUnit", _unitSpawnRate);
        }
    }

    // get random point on the map
    public Vector3 GetRandomSpawnPoint()
    {
        Vector3 target;
        do
        {
            target = new Vector3(Random.Range(-_mapSize, _mapSize), Random.Range(-_mapSize, _mapSize), _transform.position.z);
        } while (CanSpawnHere(target));

        return target;
    }

    // can the unit spawn here
    private bool CanSpawnHere(Vector3 position)
    {
        for (int i = 0; i < GameManager.Instance.restrictedAreas.Count; i++)
        {
            if (Vector3.Distance(GameManager.Instance.restrictedAreas[i].areaOrigine.position, position) <= GameManager.Instance.restrictedAreas[i].areaRadius
                && /*Replace by the player position*/ Vector3.Distance(Vector3.zero, position) <= SpawnDistanceAroundPlayer )
            {
                return true;
            }
        }
        return false;
    }

    // Set List unitOnPOI
    private void SetUnitOnPOI()
    {
        for(int i = 0;FactionManager.Instance.poi.Count > i; i++) 
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
                _unitOnPOI[FactionManager.Instance.poi.IndexOf(unit.GetComponent<UnitMovement>().targetPOI)]--;
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

}

// point used by unit for the suveillance
[System.Serializable]
public struct SurveillancePoint
{
    public Transform point;
    public GameObject unit;
}

