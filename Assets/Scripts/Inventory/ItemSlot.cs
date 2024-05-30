using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item _item = null;
    private int _quantity = 0;

    [SerializeField]
    private TextMeshProUGUI _quantityText;

    [SerializeField]
    private Image _itemSprite;

    [SerializeField]
    protected Image _itemSlotSprite;

    [SerializeField]
    public GameObject itemSelectedSprite;

    [SerializeField]
    private Inventory _inventory;

    protected bool _isAvailable = true;

    private void Start()
    {
        UpdateQuantity(_quantity);
    }

    /// <summary>
    /// Changes the item selected in inventory and activates the item slot outline
    /// </summary>
    public void GetSelected(bool isSelected)
    {
        if (isSelected)
        {
            if (_inventory.selectedItemSlot != null)
            {
                _inventory.selectedItemSlot.GetSelected(false);
            }
            _inventory.selectedItemSlot = this;
            itemSelectedSprite.SetActive(true);
        }
        else
        {
            if (_inventory.selectedItemSlot == this)
            {
                _inventory.selectedItemSlot = null;
            }
            itemSelectedSprite.SetActive(false);
        }
    }

    /// <summary>
    /// when the player hovers over the slot, get selected
    /// </summary>
    public void OnPointerEnter(PointerEventData _)
    {
        if (_isAvailable)
        {
            GetSelected(true);
        }
    }
    
    public void OnPointerExit(PointerEventData _) 
    {
        GetSelected(false);
    }

    /// <summary>
    /// method to add 1 item to the empty slot
    /// </summary>
    public void AddItemToSlot(Item item)
    {
        _item = item;
        UpdateQuantity(1);
    }

    /// <summary>
    /// Updates the sprite of the item in the slot
    /// </summary>
    public void UpdateItemSprite()
    {
        if (_item != null)
        {
            _itemSprite = _item.ItemSprite;
        }
        else
        {
            _itemSprite = null;
        }
    }

    /// <summary>
    /// Changes the quantity of the current item to "quantity"
    /// Also handles if the quantity is 0 or less (deletes the item)
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity > 0)
        {
            UpdateItemSprite();
            _quantity = quantity;
            _quantityText.text = _quantity.ToString();
            _quantityText.gameObject.SetActive(_item.IsStackable);
        }
        else
        {
            _item = null;
            UpdateItemSprite();
            _quantity = 0;
            _quantityText.gameObject.SetActive(false);
        }
    }

    public Item Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } }
    public Inventory Inventory { get { return _inventory; } set {  _inventory = value; } }
    public bool IsAvailable { get { return _isAvailable; } set { _isAvailable = value; } }
}
