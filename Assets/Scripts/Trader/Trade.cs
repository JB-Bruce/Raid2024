using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{
    private TradeData _currentData;

    [SerializeField]
    private Image _tradableItemImage;

    [SerializeField]
    private GameObject _itemRequirePrefab;

    [SerializeField]
    private Transform _itemRequireParent;

    private List<ItemRequire> itemRequires = new();

    private bool _isTradable;

    public TradeData currentData { get { return _currentData; } }

    public bool isTradable { get { return _isTradable; } }

    //configure the trade prefab
    public void Configure(TradeData tradeData)
    {
        _currentData = tradeData;

        _tradableItemImage.sprite = _currentData.tradeItem.ItemSprite;

        for (int i = 0; i < _currentData.itemsToTrade.Count; i++)
        {
            ItemToTradeData requiredItem = _currentData.itemsToTrade[i];
            GameObject requiredItemGO = Instantiate(_itemRequirePrefab, _itemRequireParent);
            itemRequires.Add(requiredItemGO.GetComponent<ItemRequire>());

            int quantityInInventory = 3;//call the methode in inventory to get the quantity of the item in inventory

            itemRequires[i].itemRequireImage.sprite = requiredItem.tradeItem.ItemSprite;
            itemRequires[i].quantityRequire.text = requiredItem.quantityNeed.ToString();
            itemRequires[i].quantityInInventory.text = quantityInInventory.ToString();

            if (quantityInInventory >= requiredItem.quantityNeed)
            {
                _isTradable = true;
            }
            else
            {
                _isTradable = false;
            }
        }
    }

    //refresh the quantity in inventory of the items required
    public void Refresh()
    {
        _isTradable = true;
        foreach (var itemRequire in itemRequires)
        {
            int quantityInInventory = /*call the methode in inventory to get the quantity of the item in inventory*/int.Parse(itemRequire.quantityInInventory.text);
            //itemRequire.quantityInInventory.text = quantityInInventory.ToString();

            if (quantityInInventory < int.Parse(itemRequire.quantityRequire.text) /*add condition inventory is not full*/ )
            {
                _isTradable = false;
            }
        }
    }
}
