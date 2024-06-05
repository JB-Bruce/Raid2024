using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestItems", menuName = "ScriptableObjects/QuestAction/QuestItems", order = 1)]
[System.Serializable]
public class QuestItems : QuestAction
{
    [SerializeField]
    private List<ItemWithQuantity> _itemsNeed = new();

    [SerializeField]
    private List<ItemWithQuantity> _itemsInInventory = new();

    //call when the QuestItems is the current QuestAction to configure it
    public override void Configure()
    {
        _itemsInInventory.Clear();
        foreach (var itemNeed in _itemsNeed)
        {
            ItemWithQuantity itemWithQuantity = new ItemWithQuantity();
            itemWithQuantity.item = itemNeed.item;
            itemWithQuantity.quantityNeed = Inventory.Instance.CountItemInInventory(itemNeed.item);
            _itemsInInventory.Add(itemWithQuantity);
        }
    }

    public override string GetObjectivesText()
    {
        string textToReturn = string.Empty;
        for (int i = 0; i <_itemsInInventory.Count; i++)
        {
            textToReturn += _itemsInInventory[i].item.Name + "   " + _itemsInInventory[i].quantityNeed + "/" + _itemsNeed[i].quantityNeed;
        }
        return textToReturn;
    }

    //check if the item is need to update the quantity
    private void CheckItem(ItemWithQuantity itemWithQuantity)
    {
        for (int i = 0; i < _itemsNeed.Count; i++)
        {
            if (_itemsNeed[i].item == itemWithQuantity.item)
            {
                ItemWithQuantity tempiItemWithQuantity = new ItemWithQuantity();
                tempiItemWithQuantity.item = _itemsNeed[i].item;
                tempiItemWithQuantity.quantityNeed = _itemsInInventory[i].quantityNeed + itemWithQuantity.quantityNeed;
                _itemsInInventory[i] = tempiItemWithQuantity;
                return;
            }
        }
    }

    //return if the QuestItems is finished
    public bool IsFinished(ItemWithQuantity itemWithQuantity)
    {
        CheckItem(itemWithQuantity);
        for (int i = 0; i < _itemsNeed.Count; i++)
        {
            if (_itemsInInventory[i].quantityNeed < _itemsNeed[i].quantityNeed)
            {
                return false;
            }
        }
        return true;
    }
}