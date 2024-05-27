using System.Collections.Generic;
using TMPro;
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

    private int evaluateUpdate = 0;
    public int jumpUpdate = 10;



    // Start is called before the first frame update
    public void Init()
    {
        _unitCombat = GetComponent<UnitCombat>();
        _weapon = transform.GetChild(0).gameObject;
        _unitMove = GetComponent<UnitMovement>();
        _unitMove.Init();
        GetComponent<UnitCombat>().Init();

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

            // Make the Unit point his weapon to the direction of his target.


            if (_unitCombat.nearestEnemy != null)
            {
                Vector3 direction = _unitCombat.nearestEnemy.transform.position - transform.position;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }

            else if (Mathf.Abs(_agent.velocity.x) > 0.01 || Mathf.Abs(_agent.velocity.y) > 0.01)
            {
                float angle = Mathf.Atan2(_agent.velocity.y, _agent.velocity.x) * Mathf.Rad2Deg;
                _weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }



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