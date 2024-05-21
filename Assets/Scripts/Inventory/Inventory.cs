using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour, IPointerClickHandler
{
    private bool _isInventoryOpen = false;

    [SerializeField] 
    private GameObject _inventoryPanel;

    public ItemSlot selectedItemSlot = null;
    
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();
    private List<EquipementSlot> _equipementSlots = new List<EquipementSlot>();

    [SerializeField]
    private GameObject _itemSlotPrefab;

    [SerializeField]
    private GameObject _equipementSlotPrefab;

    [SerializeField]
    private Transform _itemSlotsContainer;

    [SerializeField]
    private Transform _armorSlotsContainer;

    [SerializeField]
    private int _inventoryHeight;

    [SerializeField]
    private int _inventoryWidth;

    [SerializeField]
    private Item testItem;

    private const int _itemSpacing = 95;
    private const int _armorSpacing = 200;

    /// <summary>
    /// Open and closes the inventory UI
    /// </summary>
    public void OpenInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        _inventoryPanel.SetActive(_isInventoryOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem(testItem);
        }
    }

    /// <summary>
    /// Create the inventory slots and equipement slots
    /// </summary>
    private void Start()
    {
        //inventory slots
        CreateInventorySlots();

        //armor slots
        for (int i = 0; i < 3; ++i)
        {
            GameObject newSlot = Instantiate(_equipementSlotPrefab, _armorSlotsContainer);
            newSlot.transform.localPosition = new Vector3(0, -i * _armorSpacing);
            newSlot.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            newSlot.GetComponent<EquipementSlot>().Inventory = this;
            //choose the type of armor equipable in this slot
            switch (i)
            {
                case 0: //Helmet
                    newSlot.GetComponent<EquipementSlot>().ItemType = typeof(Helmet);
                    break;
                case 1: //Chestplate
                    newSlot.GetComponent<EquipementSlot>().ItemType = typeof(Chestplate);
                    break; 
                case 2: //Leggings
                    newSlot.GetComponent<EquipementSlot>().ItemType = typeof(Leggings);
                    break;
                default: 
                    break;
            }
            _equipementSlots.Add(newSlot.GetComponent<EquipementSlot>());
        }

        //Holster slots


    }

    private void CreateInventorySlots()
    {
        for (int j = 0; j < _inventoryHeight; j++)
        {
            for (int i = 0; i < _inventoryWidth; i++)
            {
                GameObject newSlot = Instantiate(_itemSlotPrefab, _itemSlotsContainer);
                newSlot.transform.localPosition = new Vector3(i * _itemSpacing, j * _itemSpacing);
                newSlot.GetComponent<ItemSlot>().Inventory = this;
                _itemSlots.Add(newSlot.GetComponent<ItemSlot>());
            }
        }
    }

    private void AddItem(Item item)
    {
        ItemSlot itemSlot = FindFirstInventorySlotAvailable(item);
        if (itemSlot.Item == null)
        {
            itemSlot.AddItemToSlot(item);
        }
        else
        {
            itemSlot.UpdateQuantity(itemSlot.Quantity + 1);
        }
    }

    /// <summary>
    /// Uses/Equipes the item selected when clicked
    /// </summary>
    public void OnPointerClick(PointerEventData _)
    {
        if (_isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
        {
            DecideHowToUseItem();
        }
    }

    /// <summary>
    /// Decide between equiping/swapping/using the selected item
    /// Can be used by TrySwapItemsInSlot as a recursif loop for stacking items
    /// </summary>
    private void DecideHowToUseItem()
    {
        if (selectedItemSlot.GetType().IsSubclassOf(typeof(EquipementSlot))) //if the player clicks on an equipement slot
        {
            Debug.Log("test");
            TrySwapItemsInSlots(selectedItemSlot, FindFirstInventorySlotAvailable(selectedItemSlot.Item));
        }
        else //if the player clicks in the inventory
        {
            if (selectedItemSlot.Item.GetType().IsSubclassOf(typeof(Equipable)))
            {
                TrySwapItemsInSlots(selectedItemSlot, FindFirstEquipementSlotAvailable(selectedItemSlot.Item));
            }
            /*
            else if (selectedItemSlot.Item.GetType() == typeof(Consumable))
            {

            }
            */
        }
    }

    /// <summary>
    /// Try to swap items from slot1 to slot2 and verify if it can stack
    /// </summary>
    private void TrySwapItemsInSlots(ItemSlot slot1, ItemSlot slot2)
    {
        if (slot1 != null && slot2 != null)
        {
            if (slot2.GetType() != typeof(EquipementSlot))//If slot2 isn't an equipement slot (we want to swap if it is)
            {
                if (slot1.Item != null && slot2.Item != null)
                {
                    if (slot1.Item == slot2.Item)//If the items are the same
                    {
                        if (TryStackingItems(slot1, slot2) == false)//Try to stack and if slot1 still has items, start again
                        {
                            DecideHowToUseItem();
                        }
                    }
                }
            }
            else//Else Swap items around
            {
                Item TempItem = slot1.Item;
                slot1.Item = slot2.Item;
                slot2.Item = TempItem;

                int TempQuantity = slot1.Quantity;
                slot1.UpdateQuantity(slot2.Quantity);
                slot2.UpdateQuantity(TempQuantity);
            }
        }
    }

    /// <summary>
    /// Try to stack the items from slot1 into slot2
    /// Returns true if slot1 is empty by the end
    /// Returns false if slot1 has some items by the end
    /// </summary>
    private bool TryStackingItems(ItemSlot slot1, ItemSlot slot2)
    {
        bool isSlotEmpty = true;

        int remainingItems = slot2.Item.MaxStack - slot2.Quantity;
        if (remainingItems > slot1.Quantity)//if slot1 won't be empty
        {
            isSlotEmpty = false;
            slot2.UpdateQuantity(slot2.Item.MaxStack);
        }
        else
        {
            slot2.UpdateQuantity(slot2.Quantity + slot1.Quantity);
        }

        slot1.UpdateQuantity(slot1.Quantity - remainingItems);

        return isSlotEmpty;
    }

    /// <summary>
    /// Find the first available slot in the inventory (empty or holding the same item as item)
    /// </summary>
    private ItemSlot FindFirstInventorySlotAvailable(Item item)
    {
        foreach (ItemSlot itemSlot in _itemSlots)
        {
            if (itemSlot.Item == null || (item.GetType().IsSubclassOf(typeof(Equipable)) && itemSlot.Item == item))
            {
                return itemSlot;
            }
        }
        return null;
    }

    /// <summary>
    /// Find the first available slot in the equipements (empty or of the same type as the item)
    /// </summary>
    private ItemSlot FindFirstEquipementSlotAvailable(Item item)
    {
        foreach (EquipementSlot equipementSlot in _equipementSlots)
        {
            if (equipementSlot.ItemType == item.GetType())
            {
                return equipementSlot;
            }
        }
        return null;
    }
}
