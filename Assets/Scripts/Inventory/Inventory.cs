using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public bool isInventoryOpen = false;
    public bool isHalfInvenoryOpen = false;

    [SerializeField] private PlayerInput _playerInput;

    private EventSystem _eventSystem;

    [SerializeField] 
    private GameObject _inventoryPanel;
    
    [SerializeField] 
    private GameObject _itemSlotsGameObject;

    public ItemSlot selectedItemSlot = null;
    
    private List<ItemSlot> _itemSlots = new List<ItemSlot>();
    public List<EquipementSlot> equipementSlots = new List<EquipementSlot>();
    public List<EquipementSlot> weaponSlots = new List<EquipementSlot>();

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

    [SerializeField]
    private Transform _itemSlotsPosInInventory;

    [SerializeField]
    private Transform _itemSlotsPosInTrade;

    public Transform containerSlotsTransform;

    public Container currentContainer = null;

    private const int _itemSpacing = 95;
    private const int _armorSpacing = 200;
    private const int _weaponSpacing = 100;

    private bool _isMoving = false;
    private Vector2 _moveDirection = Vector2.zero;

    [SerializeField]
    private MovePlayer _player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Create the inventory slots and equipement slots
    /// </summary>
    private void Start()
    {
        _eventSystem = EventSystem.current;

        //inventory slots
        CreateInventorySlots();

        //armor slots
        CreateArmorSlots();

        //Holster and Weapons slots
        CreateWeaponSlots();

        //Show the weapons in game
        equipementSlots[equipementSlots.Count - 1].gameObject.SetActive(false);
        _weaponSlotsGameObject.transform.position = _weaponSlotsPosInGame.position;
        weaponSlots[0].GetSelected(true);
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveInInventory();
        }
    }


    /// <summary>
    /// Handles controller support for the inventory
    /// </summary>
    private void MoveInInventory()
    {
        if (selectedItemSlot == null)
        {
            if (isHalfInvenoryOpen)
            {
                if (_eventSystem.currentSelectedGameObject == null)
                {
                    selectedItemSlot = _itemSlots[0];
                    selectedItemSlot.GetSelected(true);
                }
            }
            else
            {
                if (currentContainer != null)
                {
                    selectedItemSlot = currentContainer.itemSlots[0];
                }
                else
                {
                    selectedItemSlot = _itemSlots[0];
                }
                selectedItemSlot.GetSelected(true);
            }
        }
        GameObject currentSelectedGameObject = _eventSystem.currentSelectedGameObject;
        ItemSlot itemSlot = null;
        if (currentSelectedGameObject != null && currentSelectedGameObject.TryGetComponent<ItemSlot>(out itemSlot))
        {
            if (selectedItemSlot != null)
            {
                selectedItemSlot.GetSelected(false);
            }
            selectedItemSlot = itemSlot;
            selectedItemSlot.GetSelected(true);
        }
        else
        {
            if (selectedItemSlot != null)
            {
                selectedItemSlot.GetSelected(false);
            }
            selectedItemSlot = null;
        }
    }

    /// <summary>
    /// Input Action to open the inventory
    /// </summary>
    public void OpenInventoryAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OpenFullInventory();
        }
    }

    /// <summary>
    /// Open and closes the inventory UI and positions the weapon slots depending on "isInventoryOpen"
    /// </summary>
    public void OpenFullInventory()
    {
        if (_playerInput.actions.FindActionMap("InGame").enabled || _playerInput.actions.FindActionMap("Inventory").enabled)
        {
            isInventoryOpen = !isInventoryOpen;
            _inventoryPanel.SetActive(isInventoryOpen);
            int actualWeapon = _player.GetSelectedWeapon();
            _itemSlotsGameObject.SetActive(isInventoryOpen);

            if (isInventoryOpen)//Show the weapons in inventory (change position and show the holster)
            {
                Cursor.visible = true;
                _playerInput.actions.FindActionMap("InGame").Disable();
                _playerInput.actions.FindActionMap("Inventory").Enable();
                
                weaponSlots[actualWeapon].GetSelected(false);

                equipementSlots[equipementSlots.Count - 1].gameObject.SetActive(true);
                _weaponSlotsGameObject.transform.position = _weaponSlotsPosInInventory.position;
            }
            else//Show the weapons in game (change position and hide the holster)
            {
                Cursor.visible = false;
                _playerInput.actions.FindActionMap("Inventory").Disable();
                _playerInput.actions.FindActionMap("InGame").Enable();
                
                weaponSlots[actualWeapon].GetSelected(true);

                equipementSlots[equipementSlots.Count - 1].gameObject.SetActive(false);
                _weaponSlotsGameObject.transform.position = _weaponSlotsPosInGame.position;
                if (currentContainer != null)
                {
                    currentContainer.CloseContainer();
                    currentContainer = null;
                }
            }
        }
    }

    /// <summary>
    /// Opens/Closes only the inventory slots but not the equipement slots and weapon slots
    /// </summary>
    public void OpenInventory(bool state)
    {
        isHalfInvenoryOpen = state;
        _itemSlotsGameObject.SetActive(isHalfInvenoryOpen);
        int actualWeapon = _player.GetSelectedWeapon();
        if (isHalfInvenoryOpen)//Hides the weapon slots
        {
            Cursor.visible = true;
            weaponSlots[actualWeapon].GetSelected(false);
            _weaponSlotsGameObject.SetActive(false);
            _itemSlotsGameObject.transform.position = _itemSlotsPosInTrade.position;
        }
        else//Shows the weapon slots
        {
            Cursor.visible = false;
            _weaponSlotsGameObject.SetActive(true);
            weaponSlots[actualWeapon].GetSelected(true);
            _itemSlotsGameObject.transform.position = _itemSlotsPosInInventory.position;
        }
    }

    /// <summary>
    /// Handles the movement in the inventory menu.
    /// </summary>
    public void HandleMovementInInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isMoving = true;
        }
        if (context.canceled)
        {
            _isMoving = false;
        }
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
            equipementSlots.Add(newSlot.GetComponent<EquipementSlot>());
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
                equipementSlots.Add(newSlot.GetComponent<EquipementSlot>());
            }
            else
            {
                newSlot.GetComponent<EquipementSlot>().ChangeAvailability(false);
                weaponSlots.Add(newSlot.GetComponent<EquipementSlot>());
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
    public bool AddItem(Item item)
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
            return true;
        }
        return false;
    }

    /// <summary>
    /// Uses/Equipes the item selected when left clicked.
    /// </summary>
    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isInventoryOpen && selectedItemSlot != null && selectedItemSlot.Item != null)
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
            if ((isInventoryOpen || isHalfInvenoryOpen) && selectedItemSlot != null && selectedItemSlot.Item != null)
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
        if (!_itemSlots.Contains(selectedItemSlot) && !weaponSlots.Contains(selectedItemSlot) && !equipementSlots.Contains(selectedItemSlot))
        {
            TrySwapItemsInSlots(selectedItemSlot, FindFirstInventorySlotAvailable(selectedItemSlot.Item));
        }
        else if (selectedItemSlot.GetType() == typeof(EquipementSlot)) //if the player clicks on an equipement slot
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
                    for (int i = 1; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i].Item != null)//check if there are any weapons, in which case cancel swap
                        {
                            return;
                        }
                    }
                    _player.SelectDefaultWeapon();
                    for (int i = 1; i < weaponSlots.Count; i++)//reset weapon slots availability
                    {
                        weaponSlots[i].ChangeAvailability(false);
                    }
                }
            }
            if (slot2.GetType() == typeof(EquipementSlot))
            {
                EquipementSlot equipementSlot = (EquipementSlot)slot2;
                if (equipementSlot.ItemType == typeof(Holster))//if slot2 is an holster (meaning slot1 has an holster item)
                {
                    Holster holster = (Holster) slot1.Item;
                    for (int i = holster.HolsterTier; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i].Item != null)//check if there are any weapons, in which case cancel swap
                        {
                            return;
                        }
                    }
                    _player.SelectDefaultWeapon();
                    for (int i = 1; i < weaponSlots.Count; i++)//reset the availability of all weapon slot (prevents bugs)
                    {
                        weaponSlots[i].ChangeAvailability(false);
                    }
                    for (int i = 1; i < holster.HolsterTier; i++)//changes the weapon slots to available depending on the tier of holster
                    {
                        weaponSlots[i].ChangeAvailability(true);
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
            if (item.IsStackable)
            {
                if (itemSlot.Item == item && itemSlot.Quantity < itemSlot.Item.MaxStack)
                {
                    return itemSlot;
                }
            }
        }
        foreach (ItemSlot itemSlot in _itemSlots)
        {
            if (itemSlot.Item == null)
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
        foreach (EquipementSlot equipementSlot in equipementSlots)
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
        foreach (EquipementSlot weaponSlot in weaponSlots)
        {
            if (weaponSlot.Item == null && weaponSlot.IsAvailable)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    /// <summary>
    /// Finds if the inventory is full (if there is no empty inventory slot)
    /// </summary>
    public bool IsInventoryFull()
    {
        foreach (ItemSlot itemSlot in _itemSlots)
        {
            if (itemSlot.Item == null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Counts the number of item "item" in the inventory and returns it
    /// </summary>
    public int CountItemInInventory(Item item)
    {
        int count = 0;

        foreach (ItemSlot itemSlot in _itemSlots)
        {
            if (itemSlot.Item == item)
            {
                count += itemSlot.Quantity;
            }
        }

        return count;
    }

    /// <summary>
    /// Removes the quantity "quantity" of last instance of the item "item" in inventory
    /// </summary>
    public void RemoveItems(Item item, int quantity)
    {
        for (int i = _itemSlots.Count-1; i >= 0; i--)
        {
            if (quantity <= 0)
            {
                return;
            }
            if (_itemSlots[i].Item != null)
            {
                if (_itemSlots[i].Item == item && _itemSlots[i].Quantity > 0)
                {
                    if (quantity > _itemSlots[i].Quantity)
                    {
                        quantity -= _itemSlots[i].Quantity;
                        _itemSlots[i].UpdateQuantity(0);
                    }
                    else
                    {
                        _itemSlots[i].UpdateQuantity(_itemSlots[i].Quantity - quantity);
                        return;
                    }
                }
            }
        }
    }


}
