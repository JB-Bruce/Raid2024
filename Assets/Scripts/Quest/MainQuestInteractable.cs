using System.Collections.Generic;
using UnityEngine;

public class MainQuestInteractable : Interactable
{
    [SerializeField]
    private QuestManager.QuestTriggerType _questTriggerType;

    [SerializeField]
    private string _information;

    [SerializeField]
    private List<ItemWithQuantity> _itemsToGive;

    [SerializeField]
    private List<ItemWithQuantity> _itemsToDelete;

    //call when the player interact with this, return true if he can interact and the inventory is not full
    public override bool TryToInteract()
    {
        if (_canInterract)
        {
            if (Inventory.Instance.IsInventoryFull())
            {
                return false;
            }
            for (int i = 0; i < _itemsToDelete.Count; i++)
            {
                if (Inventory.Instance.CountItemInInventory(_itemsToDelete[i].item) < _itemsToDelete[i].quantityNeed)
                {
                    _canInterract = false;
                    break;
                }
            }
        }
        return base.TryToInteract();
    }

    //call when the player interact with this and this can interact
    protected override void Interact()
    {
        if (_itemsToDelete.Count > 0)
        {
            DeleteItems();
        }
        QuestManager.instance.CheckQuestTrigger(_questTriggerType, _information);
        if(_itemsToGive.Count > 0)
        {
            GiveItems();
        }    
    }

    //add the _itemsToGive in the inventory
    private void GiveItems()
    {
        for (int i = 0; i < _itemsToGive.Count; i++)
        {
            for (int j = 0; j < _itemsToGive[i].quantityNeed; j++)
            {
                Inventory.Instance.AddItem(_itemsToGive[i].item);
            } 
        }
    }

    //delete the _itemsToDelete from the inventory
    private void DeleteItems()
    {
        for (int i = 0; i < _itemsToDelete.Count; i++)
        {
            Inventory.Instance.RemoveItems(_itemsToDelete[i].item, _itemsToDelete[i].quantityNeed);
        }
    }
}