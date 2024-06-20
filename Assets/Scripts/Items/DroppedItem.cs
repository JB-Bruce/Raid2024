using UnityEngine;

public class DroppedItem : Interactable
{
    public int quantity = 0;
    public Item item;

    private float _despawnTimer = 120f;

    /// <summary>
    /// Checks if the player enter the radius of the item, if he does, gets added to the list of interactable of the player
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    /// <summary>
    /// Checks if the player leaves the radius of the item
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Highlight(false);
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }

    public override void Highlight(bool state)
    {
        if (state)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    /// <summary>
    /// Updates the sprite of the item (used only when instantiated)
    /// </summary>
    public void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().sprite = item.ItemSprite;
    }

    /// <summary>
    /// Adds itself to the player inventory when interacted with
    /// </summary>
    protected override void Interact()
    {
        Inventory inventory = Inventory.Instance;
        int tempQuantity = quantity;
        for (int i = 0; i < quantity; i++)
        {
            if (item != null)
            {
                if (!inventory.AddItem(item))
                {
                    quantity -= i;
                    if (i > 0)
                    {
                        PopUpManager.Instance.AddPopUp(item, i);
                    }
                    return;
                }
            }
        }
        PopUpManager.Instance.AddPopUp(item, tempQuantity);
        Highlight(false);
        PlayerInteraction.Instance.interactables.Remove(this);
        DroppedItemManager.Instance.droppedItems.Remove(this);
        Destroy(gameObject);
    }

    private void Start()
    {
        DroppedItemManager.Instance.droppedItems.Add(this);
    }

    private void Update()
    {
        _despawnTimer -= Time.deltaTime;
        if (_despawnTimer <= 0)
        {
            DroppedItemManager.Instance.droppedItems.Remove(this);
            Destroy(gameObject);
        }
    }
}
