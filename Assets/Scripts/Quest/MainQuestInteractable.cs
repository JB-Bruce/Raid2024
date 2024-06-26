using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainQuestInteractable : Interactable
{
    [SerializeField]
    private QuestManager.QuestTriggerType _questTriggerType;

    [SerializeField]
    private GameObject _selectedSprite;

    [SerializeField]
    private string _information;

    [SerializeField]
    private string _name;

    [SerializeField]
    private List<QuestActionItems> _itemsToGive;

    [SerializeField]
    private List<QuestActionItems> _itemsToDelete;

    [SerializeField]
    private List<QuestActionStuff> _stuffToDelete;

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
            for (int i = 0; i < _stuffToDelete.Count; i++)
            {
                if (_stuffToDelete[i].quest == questManagerCurrentQuestIndex[0] && _stuffToDelete[i].questAction == questManagerCurrentQuestIndex[1])
                {
                    for (int j = 0; j < _stuffToDelete[i].stuffsWithQuantity.Count; j++)
                    {
                        if (Inventory.Instance.GetContainerQuantityInInventory(_stuffToDelete[i].stuffsWithQuantity[j].container.Name) < _stuffToDelete[i].stuffsWithQuantity[j].quantity)
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
        if (_itemsToGive.Count > 0)
        {
            GiveItems();
        }
        if (_stuffToDelete.Count > 0)
        {
            DeleteStuffs();
        }
        if(_information == "RansomPaper")
        {
            RansomPaper.instance.OpenRansomPaper();
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
                    PopUpManager.Instance.AddPopUp(_itemsToGive[i].itemsWithQuantity[j].item, _itemsToGive[i].itemsWithQuantity[j].quantityNeed);
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

    //delete the _stuffsToDelete from the inventory
    private void DeleteStuffs()
    {
        List<int> questManagerCurrentQuestIndex = QuestManager.instance.GetCurrentMainQuestActionIndex();
        for (int i = 0; i < _stuffToDelete.Count; i++)
        {
            if (_stuffToDelete[i].quest == questManagerCurrentQuestIndex[0] && _stuffToDelete[i].questAction == questManagerCurrentQuestIndex[1])
            {
                for (int j = 0; j < _stuffToDelete[i].stuffsWithQuantity.Count; j++)
                {
                    Inventory.Instance.RemoveContainerQuantityInInventory(_stuffToDelete[i].stuffsWithQuantity[j].quantity, _stuffToDelete[i].stuffsWithQuantity[j].container.Name);
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

    public override void Highlight(bool state)
    {
        _selectedSprite.SetActive(state);
    }

    public string Name { get { return _name; } }
}

[System.Serializable]
public struct QuestActionItems
{
    public List<ItemWithQuantity> itemsWithQuantity;
    public int quest;
    public int questAction;
}

[System.Serializable]
public struct QuestActionStuff
{
    public List<StuffWithQuantity> stuffsWithQuantity;
    public int quest;
    public int questAction;
}

[System.Serializable]
public struct StuffWithQuantity
{
    public string stuff;
    public float quantity;
    public Item container;
}