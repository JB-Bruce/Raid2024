using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IPointerClickHandler
{
    private bool _isInventoryOpen = false;

    [SerializeField] 
    private GameObject _inventoryPanel;

    public ItemSlot selectedItemSlot = null;
    
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();

    [SerializeField]
    private GameObject _itemSlotPrefab;

    [SerializeField]
    private int _inventoryHeight;

    [SerializeField]
    private int _inventoryWidth;

    /// <summary>
    /// Open and closes the inventory UI
    /// </summary>
    public void OpenInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        _inventoryPanel.SetActive(_isInventoryOpen);
    }

    /// <summary>
    /// Create the inventory slots (Not Finished)
    /// </summary>
    private void Start()
    {
        for (int j = 0; j < _inventoryHeight; j++)
        {
            for (int i = 0; i < _inventoryWidth; i++)
            {
                GameObject newSlot = Instantiate(_itemSlotPrefab);
            }
        }
    }

    /// <summary>
    /// Uses the item selected when clicked
    /// </summary>
    public void OnPointerClick(PointerEventData _)
    {
        if (_isInventoryOpen && selectedItemSlot != null)
        {

        }
    }
}
