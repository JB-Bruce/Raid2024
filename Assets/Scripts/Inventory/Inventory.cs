using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool _isInventoryOpen = false;

    [SerializeField] 
    private GameObject _inventoryPanel;

    public ItemSlot selectedItemSlot = null;
    
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();
    private List<EquipementSlot> _equipementSlots = new List<EquipementSlot>();
    private List<EquipementSlot> _weaponSlots = new List<EquipementSlot>();

    [SerializeField]
    private GameObject _itemSlotPrefab;

    [SerializeField]
    private GameObject _equipementSlotPrefab;

    [SerializeField]
    private Transform _itemSlotsContainer;

    [SerializeField]
    private Transform _armorSlotsContainer;

    [SerializeField]
    private Transform _weaponSlotsContainer;

    [SerializeField]
    private int _inventoryHeight;

    [SerializeField]
    private int _inventoryWidth;

    [SerializeField]
    private Item testItem;
    private const int _itemSpacing = 95;
    private const int _armorSpacing = 200;
    private const int _weaponSpacing = 100;

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
        if (_isInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddItem(testItem);
            }
            if (Input.GetMouseButtonDown(0))
            {
                HandleLeftClick();
            }
            if (Input.GetMouseButtonDown(1))
            {
                HandleRightClick();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                }
                else if (selectedItemSlot.GetType() == typeof(ItemSlot))
                {
                    int selectedItemIndex = _itemSlots.LastIndexOf(selectedItemSlot);
                    if (selectedItemIndex < (_inventoryWidth * (_inventoryHeight - 1)))
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _itemSlots[selectedItemIndex + _inventoryWidth];
                        selectedItemSlot.GetSelected(true);
                    }
                }
                else if (_equipementSlots.Contains(selectedItemSlot))
                {
                    int selectedItemIndex = _equipementSlots.LastIndexOf((EquipementSlot) selectedItemSlot);
                    if (selectedItemIndex < 3)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[selectedItemIndex + 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                }
                else if (selectedItemSlot.GetType() == typeof(ItemSlot))
                {
                    int selectedItemIndex = _itemSlots.LastIndexOf(selectedItemSlot);
                    if (selectedItemIndex > _inventoryWidth - 1)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _itemSlots[selectedItemIndex - _inventoryWidth];
                        selectedItemSlot.GetSelected(true);
                    }
                }
                else if (_weaponSlots.Contains(selectedItemSlot))
                {
                    selectedItemSlot.GetSelected(false);
                    selectedItemSlot = _equipementSlots[_equipementSlots.Count - 2];
                    selectedItemSlot.GetSelected(true);
                }
                else
                {
                    int selectedItemIndex = _equipementSlots.LastIndexOf((EquipementSlot) selectedItemSlot);
                    if (selectedItemIndex > 0)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[selectedItemIndex - 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                }
                else if (selectedItemSlot.GetType() == typeof(ItemSlot)) 
                {
                    int selectedItemIndex = _itemSlots.LastIndexOf(selectedItemSlot);
                    if (selectedItemIndex % _inventoryWidth != _inventoryWidth - 1)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _itemSlots[selectedItemIndex + 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
                else if (_equipementSlots.Where(x => x.ItemType != typeof(Holster)).Contains(selectedItemSlot))
                {
                    selectedItemSlot.GetSelected(false);
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
                }
                else
                {
                    if (selectedItemSlot == _equipementSlots[_equipementSlots.Count - 1])
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _weaponSlots[0];
                        selectedItemSlot.GetSelected(true);
                    }
                    else
                    {
                        int selectedItemIndex = _weaponSlots.LastIndexOf((EquipementSlot) selectedItemSlot);
                        if (selectedItemIndex < 2)
                        {
                            selectedItemSlot.GetSelected(false);
                            selectedItemSlot = _weaponSlots[selectedItemIndex + 1];
                            selectedItemSlot.GetSelected(true);
                        }
                        else
                        {
                            selectedItemSlot.GetSelected(false);
                            selectedItemSlot = _itemSlots[0];
                            selectedItemSlot.GetSelected(true);
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                }
                else if (selectedItemSlot.GetType() == typeof(ItemSlot))
                {
                    int selectedItemIndex = _itemSlots.LastIndexOf(selectedItemSlot);
                    if (selectedItemIndex % _inventoryWidth != 0)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _itemSlots[selectedItemIndex - 1];
                        selectedItemSlot.GetSelected(true);
                    }
                    else
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[0];
                        selectedItemSlot.GetSelected(true);
                    }
                }
                else if (_weaponSlots.Contains(selectedItemSlot))
                {
                    int selectedItemIndex = _weaponSlots.LastIndexOf((EquipementSlot) selectedItemSlot);
                    if (selectedItemIndex > 0)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _weaponSlots[selectedItemIndex - 1];
                        selectedItemSlot.GetSelected(true);
                    }
                    else
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[_equipementSlots.Count - 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
            }
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
        for (int i = 0; i < 4; ++i)
        {
            GameObject newSlot = Instantiate(_equipementSlotPrefab, _weaponSlotsContainer);
            newSlot.transform.localPosition = new Vector3(i * _weaponSpacing, 0);
            if (i == 0)
            {
                newSlot.transform.localPosition = new Vector3(-_weaponSpacing/2, 0);
                newSlot.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                newSlot.GetComponent<EquipementSlot>().ItemType = typeof(Holster);
                _equipementSlots.Add(newSlot.GetComponent<EquipementSlot>());
            }
            else
            {
                _weaponSlots.Add(newSlot.GetComponent<EquipementSlot>());
            }
            newSlot.GetComponent<EquipementSlot>().Inventory = this;
        }
    }

    private void CreateInventorySlots()
    {
        for (int j = 0; j < _inventoryHeight; j++)
        {
            for (int i = 0; i < _inventoryWidth; i++)
            {
                GameObject newSlot = Instantiate(_itemSlotPrefab, _itemSlotsContainer);
                newSlot.transform.localPosition = new Vector3(i * _itemSpacing, -j * _itemSpacing);
                newSlot.GetComponent<ItemSlot>().Inventory = this;
                _itemSlots.Add(newSlot.GetComponent<ItemSlot>());
            }
        }
    }

    private void AddItem(Item item)
    {
        ItemSlot itemSlot = FindFirstInventorySlotAvailable(item);
        if (itemSlot != null)
        {
            if (itemSlot.Item == null)
            {
                itemSlot.AddItemToSlot(item);
            }
            else
            {
                itemSlot.UpdateQuantity(itemSlot.Quantity + 1);
            }
        }
    }

    /// <summary>
    /// Uses/Equipes the item selected when clicked
    /// </summary>
    public void HandleLeftClick()
    {
        if (_isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
        {
            DecideHowToUseItem();
        }
    }

    public void HandleRightClick()
    {
        if (_isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
        {
            TryToDeleteItem(selectedItemSlot);
        }
    }

    private void TryToDeleteItem(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null)
        {
            if (itemSlot.GetType() != typeof(EquipementSlot) && itemSlot.Item.GetType() != typeof(QuestItem))
            {
                itemSlot.UpdateQuantity(0);
            }
        }
    }

    /// <summary>
    /// Decide between equiping/swapping/using the selected item
    /// Can be used by TrySwapItemsInSlot as a recursif loop for stacking items
    /// </summary>
    private void DecideHowToUseItem()
    {
        if (selectedItemSlot.GetType() == typeof(EquipementSlot)) //if the player clicks on an equipement slot
        {
            ItemSlot temp = FindFirstInventorySlotAvailable(selectedItemSlot.Item);
            TrySwapItemsInSlots(selectedItemSlot, temp);
        }
        else //if the player clicks in the inventory
        {
            if (selectedItemSlot.Item.GetType().IsSubclassOf(typeof(Equipable)))
            {
                if (selectedItemSlot.Item.GetType().IsSubclassOf(typeof(Weapon)))
                {
                    TrySwapItemsInSlots(selectedItemSlot, FindFirstWeaponSlotAvailable(selectedItemSlot.Item));
                }
                else
                {
                    TrySwapItemsInSlots(selectedItemSlot, FindFirstEquipementSlotAvailable(selectedItemSlot.Item));
                }
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
                        return;//We stop here if we at least tried to stack
                    }
                }
            }
            //Else, we try to swap the items

            Item TempItem = slot1.Item;
            slot1.Item = slot2.Item;
            slot2.Item = TempItem;

            int TempQuantity = slot1.Quantity;
            slot1.UpdateQuantity(slot2.Quantity);
            slot2.UpdateQuantity(TempQuantity);
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
        if (remainingItems < slot1.Quantity)//if slot1 won't be empty
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
            if (itemSlot.Item == null)
            {
                return itemSlot;
            }
            if (item.IsStackable)
            {
                if (itemSlot.Item == item && itemSlot.Quantity < itemSlot.Item.MaxStack)
                {
                    return itemSlot;
                }
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

    private ItemSlot FindFirstWeaponSlotAvailable(Item item)
    {
        foreach (EquipementSlot weaponSlot in _weaponSlots)
        {
            if (weaponSlot.Item == null)
            {
                return weaponSlot;
            }
        }
        return null;
    }

}
