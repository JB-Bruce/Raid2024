using System;
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
    UnitBT _tree;
    Tuple<float,float> _waitingTime = Tuple.Create(0.5f, 3f);

    public HasBeenReached(NavMeshAgent agent, UnitBT tree)
    {
        _agent = agent;
        _tree = tree;
    }

    public override NodeState Evaluate()
    {   
        if ((_agent.pathStatus == NavMeshPathStatus.PathComplete && _agent.remainingDistance < 0.03f) || (_agent.pathStatus == NavMeshPathStatus.PathPartial && _agent.velocity == Vector3.zero)) 
        {

            _tree.canMove = !_tree.canMove;

            if(!_tree.canMove) // Set the wait Time of the unit, so that the unit waits x second
            {
                _tree.waitTime = Time.time + UnityEngine.Random.Range(_waitingTime.Item1, _waitingTime.Item2);
                return NodeState.FAILURE;
            }

            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
