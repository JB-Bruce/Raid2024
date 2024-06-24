using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item _item;
    private int _quantity;
    private float _quantityContainer;

    [SerializeField]
    private TextMeshProUGUI _quantityText;

    //[SerializeField]
    //private TextMeshProUGUI _tierText;

    [SerializeField]
    private GameObject _quantityContainerText;

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
        _inventory = Inventory.Instance;
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
            EventSystem.current.SetSelectedGameObject(gameObject);
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
        if (_isAvailable && (_inventory.isInventoryOpen || _inventory.isHalfInvenoryOpen))
        {
            GetSelected(true);
        }
    }
    
    public void OnPointerExit(PointerEventData _) 
    {
        if (_isAvailable && (_inventory.isInventoryOpen || _inventory.isHalfInvenoryOpen))
        {
            GetSelected(false);
        }
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
            _itemSprite.sprite = _item.ItemSprite;
            _itemSprite.color = Color.white;


            if (_item is QuestItemContainer questItemContainer)
            {
                _quantityContainerText.SetActive(true);
                UpdateContainerQuantity();
            }
            else
            {
                _quantityContainerText.SetActive(false);
            }
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = new Color(0.5f, 0.5f, 0.5f, 0);
        }
    }

    /// <summary>
    /// Changes the quantity of the current item to "quantity"
    /// Also handles if the quantity is 0 or less (deletes the item)
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity > 0 && _item != null)
        {
            UpdateItemSprite();
            _quantity = quantity;
            _quantityText.text = _quantity.ToString();
            _quantityText.gameObject.SetActive(_item.IsStackable);
            //_tierText.gameObject.SetActive(false);
            if (_item is Equipable equipable)
            {
                //_tierText.text = equipable.EquipementTier.ToString();
                //_tierText.gameObject.SetActive(true);
            }
        }
        else
        {
            _item = null;
            UpdateItemSprite();
            _quantity = 0;
            _quantityText.gameObject.SetActive(false);
            _quantityContainerText.SetActive(false);
        }
    }

    //Update the container quantity text
    public void UpdateContainerQuantity()
    {
        _quantityContainerText.GetComponent<TextMeshProUGUI>().text = _quantityContainer.ToString();
    }

    //Add a quantity in the item in this if it's a container
    public void AddContainerQuantity(float quantityAdd)
    {
        if(_item is QuestItemContainer questItemContainer) 
        {
            if (_quantityContainer + quantityAdd >= questItemContainer.GetQuantityFull())
            {
                _quantityContainer = questItemContainer.GetQuantityFull();
                questItemContainer.SetIsFull(true);
            }
            else
            {
                _quantityContainer += quantityAdd;
                questItemContainer.SetIsFull(false);
            }
        }
    }

    //modify the container quantity
    public void SetContainerQuantity(float quantity)
    {
        if (_item is QuestItemContainer questItemContainer)
        {
            if (quantity >= questItemContainer.GetQuantityFull())
            {
                _quantityContainer = questItemContainer.GetQuantityFull();
                questItemContainer.SetIsFull(true);
            }
            else
            {
                _quantityContainer = quantity;
                questItemContainer.SetIsFull(false);
            }
        }
    }

    //return the container quantity
    public float GetContainerQuantity()
    {
        if (_item is QuestItemContainer questItemContainer)
        {
            return _quantityContainer;
        }
        return 0;
    }

    public Item Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } }
    public Inventory Inventory { get { return _inventory; } set {  _inventory = value; } }
    public bool IsAvailable { get { return _isAvailable; } set { _isAvailable = value; } }
}