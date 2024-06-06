using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{
    private TradeData _currentData;

    private Inventory _inventory;

    [SerializeField]
    private Image _tradableItemImage;

    [SerializeField]
    private TextMeshProUGUI _tradableItemQuantityText;

    [SerializeField]
    private GameObject _itemRequirePrefab;

    [SerializeField]
    private Transform _itemRequireParent;

    private List<ItemRequire> itemRequires = new();

    private Tradable _tradable;

    public TradeData currentData { get { return _currentData; } }

    public Tradable Tradable { get { return _tradable; } }

    //configure the trade prefab
    public void Configure(TradeData tradeData)
    {
        _inventory = Inventory.Instance;

        _currentData = tradeData;

        _tradableItemImage.sprite = _currentData.tradeItem.ItemSprite;

        _tradableItemQuantityText.text = _currentData.tradeItemQuantity.ToString();

        ResetTradable();

        for (int i = 0; i < _currentData.itemsToTrade.Count; i++)
        {
            ItemWithQuantity requiredItem = _currentData.itemsToTrade[i];
            GameObject requiredItemGO = Instantiate(_itemRequirePrefab, _itemRequireParent);
            itemRequires.Add(requiredItemGO.GetComponent<ItemRequire>());

            int quantityInInventory = _inventory.CountItemInInventory(requiredItem.item);

            itemRequires[i].itemRequireImage.sprite = requiredItem.item.ItemSprite;
            itemRequires[i].quantityRequire.text = requiredItem.quantityNeed.ToString();
            itemRequires[i].quantityInInventory.text = quantityInInventory.ToString();

            if (quantityInInventory < requiredItem.quantityNeed)
            {
                _tradable.isTradable = false;
                _tradable.isMissingResources = true;
            }
            else if (_inventory.IsInventoryFull())
            {
                _tradable.isTradable = false;
                _tradable.isInventoryFull = true;
            }
        }
    }

    //refresh the quantity in inventory of the items required
    public void Refresh()
    {
        ResetTradable();

        for (int i = 0; i < itemRequires.Count; i++) 
        {
            ItemRequire itemRequire = itemRequires[i];
            int quantityInInventory = _inventory.CountItemInInventory(_currentData.itemsToTrade[i].item);
            itemRequire.quantityInInventory.text = quantityInInventory.ToString();

            if (quantityInInventory < int.Parse(itemRequire.quantityRequire.text))
            {
                _tradable.isTradable = false;
                _tradable.isMissingResources = true;
            }
        }
        if (_inventory.IsInventoryFull())
        {
            _tradable.isTradable = false;
            _tradable.isInventoryFull = true;
        }
    }

    private void ResetTradable()
    {
        _tradable.isTradable = true;
        _tradable.isMissingResources = false;
        _tradable.isInventoryFull = false;
    }
}

public struct Tradable
{
    public bool isTradable;
    public bool isInventoryFull;
    public bool isMissingResources;
}
