using UnityEngine;
using static QuestManager;

public class Tank : Interactable
{
    [SerializeField]
    private Item _itemToFill;

    [SerializeField]
    private GameObject _tankFull;

    [SerializeField]
    private GameObject _tankEmpty;

    [SerializeField]
    private float _quantity;

    [SerializeField]
    private string _stuffToPick;

    //call when the player interact with this, return true if he can interact
    public override bool TryToInteract()
    {
        if (_canInterract)
        {
            if (Inventory.Instance.CountItemInInventory(_itemToFill) == 0)
            {
                _canInterract = false;
            }
        }
        return base.TryToInteract();
    }

    //call when the player interact with this and this can interact
    protected override void Interact() 
    {
        _canInterract = false;
        _tankFull.SetActive(false);
        _tankEmpty.SetActive(true);
        Inventory.Instance.AddQuantityInQuestItemContainer(_itemToFill.Name, _quantity);
        QuestManager.instance.CheckQuestPick(_quantity, _stuffToPick);
    }

    // Checks if the player enter the radius of the tank, if he does, gets added to the list of container of the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    // Checks if the player leaves the radius of the quest interractable
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Highlight(false);
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }
}
