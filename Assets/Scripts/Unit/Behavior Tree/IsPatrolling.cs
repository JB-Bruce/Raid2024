using static Node_script;

/// <summary>
/// Is the state of the Unit Patrol
/// </summary>
public class IsPatrolling : Node
{
    private UnitBT _tree;

    public IsPatrolling (UnitBT tree)
    {
        _tree = tree;
    }

    public override NodeState Evaluate()
    {
        if(_tree.order == UnitOrder.Patrol) 
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
