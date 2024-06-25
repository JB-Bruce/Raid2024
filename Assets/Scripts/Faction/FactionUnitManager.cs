using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class FactionUnitManager : MonoBehaviour
{
    private int indexeur = 0;
    [SerializeField]
    private Transform _player;


    public Faction faction;
    public GameObject unit;
    private Transform _transform;
    public Transform parent;
    private float _mapSize;
    public float SpawnDistanceAroundPlayer = 50;
    public float womenPercentage = 0;
    public List<DrawWeapon> drawWeapons = new();
    public List<WeaponsDrawLevel> WeaponSpawnPerLevel = new();
    private int _lastFollower = 5;
    public Formation formation;

    [Header("Unit Management")]
    public List<GameObject> units = new List<GameObject>();
    public List<SurveillancePoint> surveillancePoints = new();
    private int _numberOfGuard = 0;
    public int maxUnit = 10;
    public int maxGuard = 3;
    public int nbrOfDeadUnit = 0;
    public float unitSpawnRate = 30f;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] [Range(0,100)] private float _banditSpawnInCamp = 50;
    [SerializeField] private int _formationPercentage = 75;

    [Header("Guard Point")]
    [SerializeField] private Transform _point;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    [Header("POI")]
    [SerializeField] private int _maxUnitPerPOI = 5;
    [SerializeField] private int _maxUnitOnPOI = 10;
    private int _numberOfPOIUnit = 0;
    private List<int> _unitOnPOI = new List<int>();

    private GameManager _gameManager;
    private FactionManager _factionManager;

    [Header("Sprite")]
    [SerializeField] private Sprite _menHair;
    [SerializeField] private Sprite _womenHair;
    [SerializeField] private Sprite _menBody;
    [SerializeField] private Sprite _womenBody;
    [SerializeField] private Sprite _hipHuman;


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
            SpawnUnit(true);
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
    private void SpawnUnit(bool _init = false)
    {
        bool _isBandit;
        if (CheckDistanceToPlayer(_spawnPosition.position) && faction == Faction.Bandit)
        {
            _isBandit = true;
        }
        else
        {
            _isBandit = faction == Faction.Bandit && (Random.Range(0, 100) >= _banditSpawnInCamp || _init);
        }


        GameObject go = Instantiate<GameObject>(unit, _isBandit ? GetRandomSpawnPoint() : GetPointAround(_spawnPosition.position, 50), Quaternion.identity, parent);

        UnitBT unitBT = go.GetComponent<UnitBT>();
        unitBT.Init();

        SetUnitSprite(womenPercentage, go);
        indexeur++;
        go.name = faction + indexeur.ToString();
        units.Add(go);

        GiveAJob(unitBT, _transform.position, !_isBandit);
        unitBT.faction = faction;
    }

    // Get a random position around a radius
    private Vector3 GetPointAround(Vector3 position, float radius)
    {
        Vector3 randomDirection;
        float randomDistance;
        Vector3 newPosition;
        do
        {
            randomDirection = Random.insideUnitCircle;
            randomDistance = Random.Range(0, radius);
            newPosition = position + (randomDirection * randomDistance);
        } while (CantSpawnHere(newPosition));

        return position + (randomDirection * randomDistance);
    }

    // Coroutine for wait to make spawn the unit if the player is far the unit to spawn
    private bool CheckDistanceToPlayer(Vector3 position)
    {
        return Vector3.Distance(position, _player.position) > SpawnDistanceAroundPlayer;
    }

    // Make spawn a unit with parameters (for wave)
    public GameObject SpawnWaveUnit(Vector3 position, Vector3 target, float maxRange = 0, string enemyType = "")
    {

        GameObject go = Instantiate<GameObject>(unit, position, Quaternion.identity, parent);

        UnitBT unitBT = go.GetComponent<UnitBT>();
        UnitMovement unitMovement = go.GetComponent<UnitMovement>();
        unitBT.Init();

        SetUnitSprite(womenPercentage, go);
        indexeur++;
        go.name = faction + indexeur.ToString();
        units.Add(go);

        GiveAJob(unitBT, _transform.position, true);
        unitBT.faction = faction;

        unitBT.order = UnitOrder.AreaGuard;
        unitMovement.SetGuardPoint(target, 0, maxRange);

        unitBT.SetQuestType(enemyType);

        return go;
    }

    // Give a job to unit
    private void GiveAJob(UnitBT BT, Vector3 position, bool canProtectFaction)
    {
        UnitMovement movement = BT.gameObject.GetComponent<UnitMovement>();

        int _random = Random.Range(0, 100);

        if (canProtectFaction)
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

            if (_random < 50 && _numberOfGuard < maxGuard)
            {
                BT.order = UnitOrder.AreaGuard;
                movement.SetGuardPoint(_point.position, _minDistance, _maxDistance);
                _numberOfGuard++;
                return;
            }
        }

        else if(_random < 50 && _numberOfPOIUnit < _maxUnitOnPOI)
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

        if(_random < _formationPercentage && _lastFollower < units.Count)
        {
            UnitLeader _leader = BT.gameObject.AddComponent<UnitLeader>();
            _leader.formation = formation;
            int _randomFollower = Random.Range(1,4);
            for(int i = 0; i< _randomFollower; i++) 
            {
                _leader.AddFollower(units[units.Count - (i + 2)]);
            }
            _lastFollower = units.Count + 4;
        }

        BT.order = UnitOrder.Patrol;

    }

    //See if there is enouth unit on the map and make spawn one.
    public void CheckUnit()
    {
        if(nbrOfDeadUnit > 0)
        {
            nbrOfDeadUnit--;
            StartCoroutine(SpawnUnitWithDelay(unitSpawnRate));
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
        Collider2D _hit = Physics2D.OverlapCircle(position, 0.1f);

        for (int i = 0; i < _gameManager.restrictedAreas.Count; i++)
        {
            if ((Vector3.Distance(_gameManager.restrictedAreas[i].areaOrigine.position, position) <= GameManager.Instance.restrictedAreas[i].areaRadius
                || Vector3.Distance(_player.position, position) <= SpawnDistanceAroundPlayer) || !NavMesh.SamplePosition(position, out NavMeshHit hit, 0.1f, 1) 
                || !_factionManager.IsPointInRhombus(position) || _hit != null)
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
        if (_unitOnPOI[index] > _maxUnitPerPOI)
        {
            return true;
        }
        return false;
    }

    // Set all the Sprite of a unit Randomly
    public void SetUnitSprite(float womenRandom, GameObject unit)
    {
        WeaponAttack _weaponAttack = unit.transform.GetComponentInChildren<WeaponAttack>();
        _weaponAttack.EquipWeapon(RandomDrawWeapon());

        int random = UnityEngine.Random.Range(0, 100);

        Transform bodyTransform = unit.gameObject.transform.GetChild(0).GetChild(1);

        SpriteRenderer body = bodyTransform.GetComponent<SpriteRenderer>();
        SpriteRenderer hair = bodyTransform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer hip = bodyTransform.GetChild(3).GetComponent<SpriteRenderer>();

        if (random < womenRandom)
        {
            body.sprite = _womenBody;
            hair.sprite = _womenHair;
        }

        else 
        {
            body.sprite = _menBody;
            hair.sprite = _menHair;
        }
        hip.sprite = _hipHuman;

    }

    // Draw a random weapon
    private Weapon RandomDrawWeapon()
    {
        int total = 0;
        for (int i = 0; i < drawWeapons.Count; i++)
        {
            total += drawWeapons[i].percentage;
        }

        int random = UnityEngine.Random.Range(0, total + 1);
        int unitDrop = 0;

        for (int i = 0; i < drawWeapons.Count; i++)
        {
            unitDrop += drawWeapons[i].percentage;

            if (random <= unitDrop)
            {
                return drawWeapons[i].weapon;
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

// Contain the weapon draw percentage
[System.Serializable]
public struct DrawWeapon
{
    public Weapon weapon;
    public int percentage;
}
