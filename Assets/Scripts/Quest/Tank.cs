using System.Collections;
using UnityEngine;

public class Tank : Interactable
{
    [SerializeField]
    private Item _itemToFill;

    [SerializeField]
    private GameObject _selectedSprite;

    [SerializeField]
    private float _quantityFull;

    [SerializeField]
    private float _quantity;

    [SerializeField]
    private string _stuffToPick;

    [SerializeField]
    private float _quantityToFill;

    [SerializeField]
    private float _fillingFrequency;

    [SerializeField]
    private bool _canRefill = true;

    private void Start()
    {
        StartCoroutine(ReFillTheTank());
    }

    //call when the player interact with this, return true if he can interact
    public override bool TryToInteract()
    {
        
        if (Inventory.Instance.CountItemInInventory(_itemToFill) == 0)
        {
            _canInterract = false;
        }
        else
        {
            _canInterract = true;
        }
        return base.TryToInteract();
    }

    //call when the player interact with this and this can interact
    protected override void Interact() 
    {
        Inventory.Instance.AddQuantityInQuestItemContainer(_itemToFill.Name, _quantity);
        _quantity = 0;
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

    //fill the tank
    private void FillTheTank()
    {
        _quantity = Mathf.Clamp(_quantity + _quantityToFill, 0, _quantityFull);
    }

    //refill the tank with the filling frequency
    IEnumerator ReFillTheTank()
    {
        while (_canRefill)
        {
            FillTheTank();
            yield return new WaitForSeconds(_fillingFrequency);
        }
    }

    public override void Highlight(bool state)
    {
        _selectedSprite.SetActive(state);
    }
}