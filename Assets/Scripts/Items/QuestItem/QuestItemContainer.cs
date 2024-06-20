using UnityEngine;

[CreateAssetMenu(fileName = "QuestItem", menuName = "ScriptableObjects/Item/QuestItemContainer", order = 1)]
public class QuestItemContainer : QuestItem
{
    [SerializeField]
    private float _quantityFull;

    [SerializeField]
    private string _stuffInContainer;

    private bool _isFull;

    //return if this is full
    public bool IsFull()
    {
        return (_isFull);
    }

    //set the bool isfull
    public void SetIsFull(bool isFull)
    {
        _isFull = isFull;
    }

    //return the full quantity
    public float GetQuantityFull()
    {
        return _quantityFull;
    }

    //return the stuff in container
    public string GetStuffInContainer()
    {
        return _stuffInContainer;
    }
}