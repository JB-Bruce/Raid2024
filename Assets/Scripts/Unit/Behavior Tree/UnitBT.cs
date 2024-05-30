using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

/// <summary>
/// Contain all the behavior of a unit
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class UnitBT : Humanoid
{
    public UnitOrder order;
    public float waitTime = 0;
    public bool canMove = true;

    private NavMeshAgent _agent;
    private Selector _selectorRoot;
    private UnitMovement _unitMove;
    private GameObject _weapon;
    private UnitCombat _unitCombat;
    private Transform _transform;
    public GameObject body;

    private int evaluateUpdate = 0;
    public int jumpUpdate = 10;

    private static readonly Vector3 _originalScale = Vector3.one;
    private static readonly Vector3 _flippedScale = new Vector3(-1, 1, 1);
    private Transform _weaponTransform;
    private Transform _weaponAttackTransform;



    // Start is called before the first frame update
    public void Init()
    {
        _transform = transform;
        _unitCombat = GetComponent<UnitCombat>();
        _weapon = _transform.GetChild(0).gameObject;
        _unitMove = GetComponent<UnitMovement>();
        _unitMove.Init();
        _unitCombat.Init();

        _weaponTransform = _weapon.transform;
        _weaponAttackTransform = _unitCombat.weaponAttack.transform;

        _agent = GetComponent<NavMeshAgent>();

        _selectorRoot = new Selector(new List<Node>
        {
            //Attack
            new Sequence( new List<Node>
            {
                new IsEnemyDetected(this.gameObject),
                new Selector(new List<Node>
                {
                    new CanAttack(this.gameObject),
                    new GoToEnemy(this.gameObject)
                })
            }),

            //Guard
            new Sequence( new List<Node>
            {
                new CheckOrderState(this, UnitOrder.AreaGuard),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new HasBeenReached(_agent, this),
                        new Guard(this.gameObject)
                    })
                } )
            }), 

            //Patrol
            new Sequence( new List<Node>
            {
                new CheckOrderState(this, UnitOrder.Patrol),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new HasBeenReached(_agent, this),
                        new Patrol(this.gameObject)
                    })
                } )
            }),

            // Surveillance
            new Sequence( new List<Node>
            {
                new CheckOrderState(this, UnitOrder.Surveillance),
                new GoToSurveillancePoint(_unitMove)
            }),

            // Capture of POI
            new Sequence( new List<Node>
            {
                new CheckOrderState(this, UnitOrder.POICapture),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new IsPOICaptured(_unitMove),
                        new GoToSurveillancePoint(_unitMove)
                    }),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new HasBeenReached(_agent, this),
                            new Guard(this.gameObject)
                        })
                    })
                })
            })

        });

    }

    // Update is called once per frame
    void Update()
    {
        evaluateUpdate++;
        if (waitTime < Time.time && evaluateUpdate > jumpUpdate) // Is the unit wait
        {
            evaluateUpdate = 0;
            _selectorRoot.Evaluate();

            UnitBodyRotation();
            WeaponAimRotation();
        }

    }

    // Make unit see in the right direction
    private void UnitBodyRotation()
    {
        if (_agent.velocity.x < 0.05f && body.transform.localScale != _flippedScale)
        {
            body.transform.localScale = _flippedScale;
        }
        else if (_agent.velocity.x > 0.05f && body.transform.localScale != _originalScale)
        {
            body.transform.localScale = _originalScale;
        }
    }

    // Make the Unit point his weapon to the direction of his target.
    private void WeaponAimRotation()
    {
        if (_unitCombat.nearestEnemy != null)
        {
            Vector3 direction = _unitCombat.nearestEnemy.transform.position - _weaponAttackTransform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        else if (Mathf.Abs(_agent.velocity.x) > 0.01 || Mathf.Abs(_agent.velocity.y) > 0.01)
        {
            float angle = Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) * Mathf.Rad2Deg;
            _weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

}

// Contain all the possible state of a unit
public enum UnitOrder
{
    Patrol, AreaGuard, Surveillance, POICapture
}

// Contain all the possible Faction 
public enum Faction
{
    Null,Military, Utopist, Survivalist, Scientist, Bandit, Player
}