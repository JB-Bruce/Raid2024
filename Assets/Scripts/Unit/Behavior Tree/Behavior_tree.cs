using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

using static Selector;
using static Sequence;

using static HasBeenReached;
using static IsGuarding;
using static IsPatrolling;
using static Guard;

/// <summary>
/// Contain all the behavior of a unit
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class Behavior_tree : MonoBehaviour
{
    public UnitOrder order;
    private NavMeshAgent _agent;
    private Selector _selectorRoot;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _selectorRoot = new Selector(new List<Node> 
        {
            new Sequence( new List<Node> 
            {
                new IsGuarding(this),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> 
                    {
                        new HasBeenReached(_agent),
                        new Guard(this.gameObject)
                    })
                } )
            }), 

            new Sequence( new List<Node> 
            {
                new IsPatrolling(this),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new HasBeenReached(_agent),
                        new Patrol(this.gameObject)
                    })
                } )
            })
        
        });

    }

    // Update is called once per frame
    void Update()
    {
        _selectorRoot.Evaluate();
    }

    // Possible order unit can receive


}

// Contain all the possible state of a unit
public enum UnitOrder
{
    Patrol, AreaGuard, Surveillance
}