using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private GameObject _weaponSlotsGameObject;

    [SerializeField]
    private Transform _weaponSlotsPosInInventory;

    [SerializeField]
    private Transform _weaponSlotsPosInGame;

    private const int _itemSpacing = 95;
    private const int _armorSpacing = 200;
    private const int _weaponSpacing = 100;

    private const float _moveCooldown = 0.2f;
    private bool _canMove = true;
    private bool _isMoving = false;
    private Vector2 _moveDirection = Vector2.zero;

    private void Update()
    {
        if (_isMoving && _canMove)
        {
            if (_moveDirection == Vector2.down)
            {
                StartCoroutine(MoveCooldown());
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
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
                    int selectedItemIndex = _equipementSlots.LastIndexOf((EquipementSlot)selectedItemSlot);
                    if (selectedItemIndex < 3)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[selectedItemIndex + 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
            }
            if (_moveDirection == Vector2.up)
            {
                StartCoroutine(MoveCooldown());
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
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
                    int selectedItemIndex = _equipementSlots.LastIndexOf((EquipementSlot)selectedItemSlot);
                    if (selectedItemIndex > 0)
                    {
                        selectedItemSlot.GetSelected(false);
                        selectedItemSlot = _equipementSlots[selectedItemIndex - 1];
                        selectedItemSlot.GetSelected(true);
                    }
                }
            }
            if (_moveDirection == Vector2.right)
            {
                StartCoroutine(MoveCooldown());
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
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
                        int selectedItemIndex = _weaponSlots.LastIndexOf((EquipementSlot)selectedItemSlot);
                        if (_equipementSlots[3].Item == null)
                        {
                            selectedItemSlot.GetSelected(false);
                            selectedItemSlot = _itemSlots[0];
                            selectedItemSlot.GetSelected(true);
                        }
                        else
                        {
                            Holster holster = (Holster)_equipementSlots[3].Item;
                            if (selectedItemIndex < holster.HolsterTier - 1)
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
            }
            if (_moveDirection == Vector2.left)
            {
                StartCoroutine(MoveCooldown());
                if (selectedItemSlot == null)
                {
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
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
                    int selectedItemIndex = _weaponSlots.LastIndexOf((EquipementSlot)selectedItemSlot);
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
    /// Open and closes the inventory UI and positions the weapon slots depending on "_isInventoryOpen"
    /// </summary>
    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isInventoryOpen = !_isInventoryOpen;
            _inventoryPanel.SetActive(_isInventoryOpen);

            if (_isInventoryOpen)//Show the weapons in inventory (change position and show the holster)
            {
                _equipementSlots[_equipementSlots.Count - 1].gameObject.SetActive(true);
                _weaponSlotsGameObject.transform.position = _weaponSlotsPosInInventory.position;
            }
            else//Show the weapons in game (change position and hide the holster)
            {
                _equipementSlots[_equipementSlots.Count - 1].gameObject.SetActive(false);
                _weaponSlotsGameObject.transform.position = _weaponSlotsPosInGame.position;
            }
        }
    }

    /// <summary>
    /// Handles the movement in the inventory menu.
    /// </summary>
    public void HandleMovementInInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveDirection = context.ReadValue<Vector2>();
            _isMoving = true;
        }
        if (context.canceled)
        {
            _isMoving = false;
        }
    }

    private IEnumerator MoveCooldown()
    {
        _canMove = false;
        yield return new WaitForSeconds(_moveCooldown);
        _canMove = true;
    }


    /// <summary>
    /// Create the inventory slots and equipement slots
    /// </summary>
    private void Start()
    {
        //inventory slots
        CreateInventorySlots();

        //armor slots
        CreateArmorSlots();

        //Holster and Weapons slots
        CreateWeaponSlots();

        //Show the weapons in game
        _equipementSlots[_equipementSlots.Count - 1].gameObject.SetActive(false);
        _weaponSlotsGameObject.transform.position = _weaponSlotsPosInGame.position;
    }

    /// <summary>
    /// Creates the inventory slots
    /// </summary>
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

    /// <summary>
    /// Creates the armor slots and attribut their types
    /// </summary>
    private void CreateArmorSlots()
    {
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
    }

    /// <summary>
    /// Creates the weapon slots and the holster slot
    /// </summary>
    private void CreateWeaponSlots()
    {
        for (int i = 0; i < 4; ++i)
        {
            GameObject newSlot = Instantiate(_equipementSlotPrefab, _weaponSlotsContainer);
            newSlot.transform.localPosition = new Vector3(i * _weaponSpacing, 0);
            if (i == 0)
            {
                newSlot.transform.localPosition = new Vector3(-_weaponSpacing / 2, 0);
                newSlot.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                newSlot.GetComponent<EquipementSlot>().ItemType = typeof(Holster);
                _equipementSlots.Add(newSlot.GetComponent<EquipementSlot>());
            }
            else
            {
                newSlot.GetComponent<EquipementSlot>().ChangeAvailability(false);
                _weaponSlots.Add(newSlot.GetComponent<EquipementSlot>());
            }
            if (i == 1)
            {
                newSlot.GetComponent<EquipementSlot>().ChangeAvailability(true);
            }

            newSlot.GetComponent<EquipementSlot>().Inventory = this;
        }
    }

    /// <summary>
    /// Adds the item "item" to the inventory if a slot is available
    /// </summary>
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
    /// Uses/Equipes the item selected when left clicked.
    /// </summary>
    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
            {
                DecideHowToUseItem();
            }
        }
    }

    /// <summary>
    /// throw/destroy the item selected when right clicked.
    /// </summary>
    public void HandleRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
            {
                TryToDeleteItem(selectedItemSlot);
            }
        }
    }

    /// <summary>
    /// Used when throwing an item, it tries to delete the item if it's not a quest item or on an equipement slot
    /// </summary>
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
    /// Decide between equiping/swapping/using the selected item.
    /// Can be used by TrySwapItemsInSlot as a recursif loop for stacking items
    /// </summary>
    private void DecideHowToUseItem()
    {
        if (selectedItemSlot.GetType() == typeof(EquipementSlot)) //if the player clicks on an equipement slot
        {
            TrySwapItemsInSlots(selectedItemSlot, FindFirstInventorySlotAvailable(selectedItemSlot.Item));
        }
        else //if the player clicks in the inventory
        {
            if (selectedItemSlot.Item.GetType().IsSubclassOf(typeof(Equipable)))//if the item is equipable
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
            else if (selectedItemSlot.Item.GetType() == typeof(Consumable))//if the item is consumable
            {
                ConsumeItem(selectedItemSlot);
            }
        }
    }

    /// <summary>
    /// Consume the item clicked and updates the stats of the player depending on the item
    /// </summary>
    private void ConsumeItem(ItemSlot itemSlot)
    {
        Consumable item = (Consumable) itemSlot.Item;

        /* Update the stats of the player HERE
        
        Player.food += item.food
        Player.thirst += item.thirst
        
        */ 

        itemSlot.UpdateQuantity(itemSlot.Quantity-1);
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
            //Else, if slot1 or 2 aren't hoslter slots (for more checks), we swap the items

            if (slot1.GetType() == typeof(EquipementSlot))
            {
                EquipementSlot equipementSlot = (EquipementSlot) slot1;
                if (equipementSlot.ItemType == typeof(Holster))//if slot1 is an holster
                {
                    for (int i = 1; i < _weaponSlots.Count; i++)
                    {
                        if (_weaponSlots[i].Item != null)//check if there are any weapons, in which case cancel swap
                        {
                            return;
                        }
                    }
                    for (int i = 1; i < _weaponSlots.Count; i++)//reset weapon slots availability
                    {
                        _weaponSlots[i].ChangeAvailability(false);
                    }
                }
            }
            if (slot2.GetType() == typeof(EquipementSlot))
            {
                EquipementSlot equipementSlot = (EquipementSlot)slot2;
                if (equipementSlot.ItemType == typeof(Holster))//if slot2 is an holster (meaning slot1 has an holster item)
                {
                    for (int i = 1; i < _weaponSlots.Count; i++)//reset the availability of all weapon slot (prevents bugs)
                    {
                        _weaponSlots[i].ChangeAvailability(false);
                    }
                    Holster holster = (Holster) slot1.Item;
                    for (int i = 1; i < holster.HolsterTier; i++)//changes the weapon slots to available depending on the tier of holster
                    {
                        _weaponSlots[i].ChangeAvailability(true);
                    }
                }
            }
            ItemSwap(slot1, slot2);
        }
    }

    /// <summary>
    /// Swaps 2 item slots
    /// </summary>
    private void ItemSwap(ItemSlot slot1, ItemSlot slot2)
    {
        Item TempItem = slot1.Item;
        slot1.Item = slot2.Item;
        slot2.Item = TempItem;

        int TempQuantity = slot1.Quantity;
        slot1.UpdateQuantity(slot2.Quantity);
        slot2.UpdateQuantity(TempQuantity);
    }


    /// <summary>
    /// Try to stack the items from slot1 into slot2.
    /// Returns true if slot1 is empty by the end.
    /// Returns false if slot1 has some items by the end.
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

    /// <summary>
    /// Find the first empty weapon slot available
    /// </summary>
    private ItemSlot FindFirstWeaponSlotAvailable(Item item)
    {
        foreach (EquipementSlot weaponSlot in _weaponSlots)
        {
            if (weaponSlot.Item == null && weaponSlot.IsAvailable)
            {
                return weaponSlot;
            }
        }
        return null;
    }

}
