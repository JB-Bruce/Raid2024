using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionUnitManager : MonoBehaviour
{
    public Faction faction;
    public GameObject unit;
    private Transform _transform;
    public Transform parent;

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




    // Start is called before the first frame update
    void Start()
    {
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
        go.transform.position = _transform.position;
        
        units.Add(go);

        GiveAJob(unitBT, _transform.position);
        unitBT.faction = faction;
    }

    // Give a job to unit
    private void GiveAJob(UnitBT BT, Vector3 position)
    {
        UnitMovement movement = BT.gameObject.GetComponent<UnitMovement>();
        for (int i = 0; i < surveillancePoints.Count; i++) 
        {
            if (surveillancePoints[i].unit == null)
            {
                BT.order = UnitOrder.Surveillance;

                SurveillancePoint surveillancePoint = surveillancePoints[i];
                surveillancePoint.unit = BT.gameObject;
                surveillancePoints[i] = surveillancePoint;
                BT.surveillancePoint = surveillancePoints[i].point.position;
                movement.ChangeTarget(surveillancePoints[i].point.position);

                return;
            }
        }

        if(Random.Range(0,100) < 50 && _numberOfGuard < maxGuard)
        {
            BT.order = UnitOrder.AreaGuard;
            movement.SetGuardPoint(point.position, minDistance, maxDistance);
            _numberOfGuard++;
            return;
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

}

// point used by unit for the suveillance
[System.Serializable]
public struct SurveillancePoint
{
    public Transform point;
    public GameObject unit;
}

