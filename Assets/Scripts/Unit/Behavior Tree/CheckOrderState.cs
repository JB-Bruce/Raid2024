using static Node_script;

/// <summary>
/// Compare the different order 
/// </summary>
public class CheckOrderState : Node
{
    private UnitBT _tree;
    private UnitOrder _order;

    public CheckOrderState(UnitBT tree, UnitOrder order)
    {
        _tree = tree;
        _order = order;
    }

    public override NodeState Evaluate()
    {
        if (_tree.order == _order)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
