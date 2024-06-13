using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable
{
    [SerializeField] private GameObject _itemSlotPrefab;

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    public GameObject containerSelectedSprite;

    public int rerollNumbers = 1;

    public LootTable lootTable;

    [SerializeField] private List<ContainerItem> _items;

    public int containerRows = 3;

    public int containerColumn = 3;

    [SerializeField]
    private bool _isUnit = false;

    public float despawnTimer = 0f;
    public bool despawnable = false;

    public string openSFX;

    private bool _hasBeenOpened = false;

    private Inventory _inventory;

    private SoundManager _soundManager;

    private void Start()
    {
        _inventory = Inventory.Instance;
        _soundManager = SoundManager.instance;
    }

    /// <summary>
    /// Coroutine to call after spawning a container on an ennemi corpse and setting the despawn timer
    /// </summary>
    private void Update()
    {
        if (despawnable)
        {
            despawnTimer -= Time.deltaTime;
            if (despawnTimer < 0f && _inventory.currentContainer != this)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Checks if the player enter the radius of the container, if he does, gets added to the list of container of the player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    /// <summary>
    /// Checks if the player leaves the radius of the container
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Highlight(false);
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }

    /// <summary>
    /// Open the container and if it has never been opened, generates the items inside
    /// </summary>
    public void OpenContainer()
    {
        if (!_hasBeenOpened)
        {
            _hasBeenOpened = true;
            GenerateItems();
        }
        _soundManager.PlaySFX(openSFX);
        CreateItemSlots();
    }

    private void GenerateItems()
    {
        if (lootTable != null)
        {
            for (int i = 0; i < rerollNumbers; i++)
            {
                ContainerItem containerItem = lootTable.GenerateItem();
                if (containerItem.amount > 0)
                {
                    _items.Add(containerItem);
                }
            }
        }
    }

    /// <summary>
    /// Generates the item slots and places the items inside
    /// </summary>
    private void CreateItemSlots()
    {
        //Center the slots
        _inventory.containerSlotsTransform.localPosition = new Vector3(597.5f - (containerColumn - 1) * 95f / 2f, ((containerRows - 1) * 95f / 2f) + 10f, _inventory.containerSlotsTransform.localPosition.z);
        
        //Creates the item slots
        for (int i = 0; i < containerRows; i++)
        {
            for (int j = 0 ; j < containerColumn; j++)
            {
                GameObject itemSlot = Instantiate(_itemSlotPrefab, _inventory.containerSlotsTransform);
                itemSlot.transform.localPosition += new Vector3(j*95f, -i*95f, 0);
                itemSlots.Add(itemSlot.GetComponent<ItemSlot>());
            }
        }

        //Places the items inside of the slots
        for (int i = 0; i < _items.Count; i++)
        {
            for (int j = 0; j < _items[i].amount; j++)
            {
                if (!AddItemToContainer(_items[i].item))
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Try to add an item to container and return if it failed or not
    /// </summary>
    private bool AddItemToContainer(Item item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                itemSlots[i].UpdateQuantity(1);
                return true;
            }
            else if (itemSlots[i].Item.IsStackable && itemSlots[i].Item == item && itemSlots[i].Item.MaxStack > itemSlots[i].Quantity)
            {
                itemSlots[i].UpdateQuantity(itemSlots[i].Quantity + 1);
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Closes the container and destroys the item slots game object (keeps the item).
    /// ONLY CALLED BY INVENTORY
    /// </summary>
    public void CloseContainer()
    {
        _items.Clear();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item != null)
            {
                ContainerItem containerItem;
                containerItem.item = itemSlots[i].Item;
                containerItem.amount = itemSlots[i].Quantity;
                _items.Add(containerItem);
            }
            Destroy(itemSlots[i].gameObject);
        }
        itemSlots.Clear();
    }

    public override void Highlight(bool state)
    {
        if (!_isUnit)
        {
            containerSelectedSprite.SetActive(state);
        }
    }

    protected override void Interact()
    {
        _inventory.OpenFullInventory();
        _inventory.currentContainer = this;
        OpenContainer();
    }
}

[System.Serializable]
public struct ContainerItem
{
    public Item item;
    public int amount;
}