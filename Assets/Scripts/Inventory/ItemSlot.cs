using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item _item = null;
    private int _quantity = 0;

    [SerializeField]
    private UnityEngine.UI.Image _itemSprite;

    [SerializeField]
    private GameObject _itemSelectedSprite;

    [SerializeField]
    private Inventory _inventory;
    
    /// <summary>
    /// Changes the item selected in inventory and activates the item slot outline
    /// </summary>
    private void GetSelected(bool isSelected)
    {
        if (isSelected)
        {
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

    public void OnPointerExit(PointerEventData _)
    {
        GetSelected(false);
    }

    public Item Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } set { _quantity = value; } }
    public Inventory Inventory { get { return _inventory; } set {  _inventory = value; } }
}
