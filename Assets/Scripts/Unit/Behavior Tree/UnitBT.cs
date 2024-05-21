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
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _selectorRoot = new Selector(new List<Node> 
        {
            //Attack
            new Sequence( new List<Node>
            {
                new IsEnemyDetected(this.gameObject),
                new Selector(new List<Node>
                {
                    new Sequence( new List<Node>
                    {
                        new CanAttack(this.gameObject)
                    }),
                    new GoToEnemy(this.gameObject)

                })
            }),

            //Guard
            new Sequence( new List<Node> 
            {
                new IsGuarding(this),
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
                new IsPatrolling(this),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new HasBeenReached(_agent, this),
                        new Patrol(this.gameObject)
                    })
                } )
            })
        
        });

    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime < Time.time) // Is the unit wait
        {
            _selectorRoot.Evaluate();
        }

    }

    // Possible order unit can receive


}

// Contain all the possible state of a unit
public enum UnitOrder
{
    Patrol, AreaGuard, Surveillance
}