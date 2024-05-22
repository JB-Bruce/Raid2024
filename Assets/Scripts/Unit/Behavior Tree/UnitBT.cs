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

    private int evaluateUpdate = 0;
    public int jumpUpdate = 10;

    // Start is called before the first frame update
    public void Init()
    {
        GetComponent<UnitMovement>().Init();
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
        evaluateUpdate++;
        if(waitTime < Time.time && evaluateUpdate > jumpUpdate) // Is the unit wait
        {
            evaluateUpdate = 0;
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

// Contain all the possible Faction 
public enum Faction
{
    Null,Military, Utopist, Survivalist, Scientist, Bandit
}