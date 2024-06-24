using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

/// <summary>
/// Contain all the behavior of a unit
/// </summary>

public class UnitBT : Humanoid
{
    public UnitOrder order;
    public float waitTime = 0;
    public bool canMove = true;

    public UnitLeader master;

    private Selector _selectorRoot;
    private UnitMovement _unitMove;
    [SerializeField] private GameObject _weapon;
    private UnitCombat _unitCombat;
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
        //_weapon = _transform.GetChild(0).gameObject;
        _unitMove = GetComponent<UnitMovement>();
        _unitMove.Init();
        _unitCombat.Init();
        _unitCombat.weaponAttack.Init();


        _weaponTransform = _weapon.transform;
        _weaponAttackTransform = _unitCombat.weaponAttack.transform;

        _selectorRoot = new Selector(new List<Node>
        {
            //Attack
            new Sequence( new List<Node>
            {
                new IsEnemyDetected(this.gameObject),
                new Selector(new List<Node>
                {
                    new CanAttack(this.gameObject),
                    new GoToEnemy(this.gameObject, _agent)
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
                new GoToSurveillancePoint(_unitMove, _agent)
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
                        new GoToSurveillancePoint(_unitMove, _agent)
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
    protected override void Update()
    {
        base.Update();
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
        if (_agent.velocity.x < -0.1 && body.transform.localScale != _flippedScale)
        {
            body.transform.localScale = _flippedScale;
        }
        else if (_agent.velocity.x > 0.1f && body.transform.localScale != _originalScale)
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

        else if (Mathf.Abs(_agent.velocity.x) > 0.03 || Mathf.Abs(_agent.velocity.y) > 0.03)
        {
            float angle = Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) * Mathf.Rad2Deg;
            _weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

}

// Contain all the possible state of a unit
public enum UnitOrder
{
    Patrol, AreaGuard, Surveillance, POICapture, Follow
}

// Contain all the possible Faction 
public enum Faction
{
    Null,Military, Utopist, Survivalist, Scientist, Bandit, Player
}