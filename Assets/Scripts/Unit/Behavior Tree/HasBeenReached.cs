using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Node_script;

/// <summary>
/// Is the target point reached
/// </summary>
public class HasBeenReached : Node
{
    NavMeshAgent _agent;

    public HasBeenReached(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override NodeState Evaluate()
    {   
        if ((_agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.1f) || (_agent.pathStatus == NavMeshPathStatus.PathPartial && _agent.velocity == Vector3.zero)) 
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
