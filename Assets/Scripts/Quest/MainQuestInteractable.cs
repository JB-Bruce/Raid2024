using System.Collections.Generic;
using UnityEngine;

public class MainQuestInteractable : Interactable
{
    [SerializeField]
    private QuestManager.QuestTriggerType _questTriggerType;

    [SerializeField]
    private string _information;

    [SerializeField]
    private List<QuestActionItems> _itemsToGive;

    [SerializeField]
    private List<QuestActionItems> _itemsToDelete;

    //call when the player interact with this, return true if he can interact and the inventory is not full
    public override bool TryToInteract()
    {
        if (_canInterract)
        {
            List<int> questManagerCurrentQuestIndex = QuestManager.instance.GetCurrentMainQuestActionIndex();
            bool isInventoryFull = Inventory.Instance.IsInventoryFull();
            for (int i = 0; i < _itemsToGive.Count; i++)
            {
                if (_itemsToGive[i].quest == questManagerCurrentQuestIndex[0] && _itemsToGive[i].questAction == questManagerCurrentQuestIndex[1] &&
                isInventoryFull)
                {
                    return false;
                }
            }
            for (int i = 0; i < _itemsToDelete.Count; i++)
            {
                if (_itemsToDelete[i].quest == questManagerCurrentQuestIndex[0] && _itemsToDelete[i].questAction == questManagerCurrentQuestIndex[1])
                {
                    for (int j = 0; j < _itemsToDelete[i].itemsWithQuantity.Count; j++)
                    {
                        if (Inventory.Instance.CountItemInInventory(_itemsToDelete[i].itemsWithQuantity[j].item) < _itemsToDelete[i].itemsWithQuantity[j].quantityNeed)
                        {
                            return false;
                        }
                    }
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
        if(_itemsToGive.Count > 0)
        {
            GiveItems();
        }
        if(_questTriggerType != QuestManager.QuestTriggerType.dialogue)
        {
            QuestManager.instance.CheckQuestTrigger(_questTriggerType, _information);
        }
    }

    //add the _itemsToGive in the inventory
    private void GiveItems()
    {
        List<int> questManagerCurrentQuestIndex = QuestManager.instance.GetCurrentMainQuestActionIndex();
        for (int i = 0; i < _itemsToGive.Count; i++)
        {
            if (_itemsToGive[i].quest == questManagerCurrentQuestIndex[0] && _itemsToGive[i].questAction == questManagerCurrentQuestIndex[1])
            {
                for (int j = 0; j < _itemsToGive[i].itemsWithQuantity.Count; j++)
                {
                    for (int k = 0; k < _itemsToGive[i].itemsWithQuantity[j].quantityNeed; k++)
                    {
                        Inventory.Instance.AddItem(_itemsToGive[i].itemsWithQuantity[j].item);
                    }
                }
            }
        }
    }

    //delete the _itemsToDelete from the inventory
    private void DeleteItems()
    {
        List<int> questManagerCurrentQuestIndex = QuestManager.instance.GetCurrentMainQuestActionIndex();
        for (int i = 0; i < _itemsToDelete.Count; i++)
        {
            if (_itemsToDelete[i].quest == questManagerCurrentQuestIndex[0] && _itemsToDelete[i].questAction == questManagerCurrentQuestIndex[1])
            {
                for (int j = 0; j < _itemsToDelete[i].itemsWithQuantity.Count; j++)
                {
                    Inventory.Instance.RemoveItems(_itemsToDelete[i].itemsWithQuantity[j].item, _itemsToDelete[i].itemsWithQuantity[j].quantityNeed);
                }
            }
        }
    }

    // Checks if the player enter the radius of the container, if he does, gets added to the list of container of the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _questTriggerType != QuestManager.QuestTriggerType.dialogue)
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    // Checks if the player leaves the radius of the quest interractable
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _questTriggerType != QuestManager.QuestTriggerType.dialogue)
        {
            Highlight(false);
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }
}

[System.Serializable]
public struct QuestActionItems
{
    public List<ItemWithQuantity> itemsWithQuantity;
    public int quest;
    public int questAction;
}