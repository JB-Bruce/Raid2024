using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Container : MonoBehaviour
{
    [SerializeField] private GameObject _itemSlotPrefab;

    [SerializeField] private List<ItemSlot> _itemSlots = new List<ItemSlot>();

    public GameObject containerSelectedSprite;

    public int minRerollNumbers = 1;

    public int maxRerollNumbers = 1;

    public LootTable lootTable;

    [SerializeField] private List<ContainerItem> _items;

    public int containerRows = 3;

    public int containerColumn = 3;

    public float despawnTimer = 0f;

    private bool _hasBeenOpened = false;

    private Inventory _inventory;

    private void Start()
    {
        _inventory = Inventory.Instance;
    }

    /// <summary>
    /// Coroutine to call after spawning a container on an ennemi corpse and setting the despawn timer
    /// </summary>
    public IEnumerator DespawnContainer()
    {
        yield return new WaitForSeconds(despawnTimer);
        while (_inventory.isInventoryOpen)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Checks if the player enter the radius of the container, if he does, gets added to the list of container of the player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.containers.Add(this);
        }
    }

    /// <summary>
    /// Checks if the player leaves the radius of the container
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            containerSelectedSprite.SetActive(false);
            PlayerInteraction.Instance.containers.Remove(this);
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
        CreateItemSlots();
    }

    /// <summary>
    /// Generates the items from the lootTable a random amount of time between minRerollNumbers and maxRerollNumbers
    /// </summary>
    private void GenerateItems()
    {
        if (lootTable != null)
        {
            int rerollNumbers = UnityEngine.Random.Range(minRerollNumbers, maxRerollNumbers);
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
                _itemSlots.Add(itemSlot.GetComponent<ItemSlot>());
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
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (_itemSlots[i].Item == null)
            {
                _itemSlots[i].Item = item;
                _itemSlots[i].UpdateQuantity(1);
                return true;
            }
            else if (_itemSlots[i].Item.IsStackable && _itemSlots[i].Item == item && _itemSlots[i].Item.MaxStack > _itemSlots[i].Quantity)
            {
                _itemSlots[i].UpdateQuantity(_itemSlots[i].Quantity + 1);
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
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            if (_itemSlots[i].Item != null)
            {
                ContainerItem containerItem;
                containerItem.item = _itemSlots[i].Item;
                containerItem.amount = _itemSlots[i].Quantity;
                _items.Add(containerItem);
            }
            Destroy(_itemSlots[i].gameObject);
        }
        _itemSlots.Clear();
    }
}

[System.Serializable]
public struct ContainerItem
{
    public Item item;
    public int amount;
}