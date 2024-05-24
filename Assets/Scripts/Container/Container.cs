using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemSlotPrefab;

    private List<ItemSlot> _itemSlots = new List<ItemSlot>();

    [SerializeField] 
    private GameObject _containerSelectedSprite;

    public LootTable lootTable;

    private List<Item> _items;

    public float despawnTimer = 0f;

    private bool _hasBeenOpened = false;

    /// <summary>
    /// Coroutine to call after spawning a container on an ennemi corpse and setting the despawn timer
    /// </summary>
    public IEnumerator DespawnContainer()
    {
        yield return new WaitForSeconds(despawnTimer);
        Inventory inventory = Inventory.Instance;
        while (inventory.isInventoryOpen)
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
            _containerSelectedSprite.SetActive(true);
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
            _containerSelectedSprite.SetActive(false);
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
            //GenerateItems();
        }
        
    }

    /// <summary>
    /// Closes the container and destroys the item slots game object (keeps the item).
    /// ONLY CALLED BY INVENTORY
    /// </summary>
    public void CloseContainer()
    {
        for (int i = 0; i < _itemSlots.Count; i++)
        {
            Destroy(_itemSlots[i].gameObject);
        }

    }
}
