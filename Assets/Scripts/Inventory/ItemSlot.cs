using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Item _item = null;
    private int _quantity = 0;

    [SerializeField]
    private TextMeshProUGUI _quantityText;

    [SerializeField]
    private Image _itemSprite;

    [SerializeField]
    private GameObject _itemSelectedSprite;

    [SerializeField]
    private Inventory _inventory;


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
            _itemSelectedSprite.SetActive(true);
        }
        else
        {
            _inventory.selectedItemSlot = null;
            _itemSelectedSprite.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData _)
    {
        GetSelected(true);
    }
    
    public void AddItemToSlot(Item item)
    {
        _item = item;
        UpdateQuantity(1);
    }

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
}
