using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    private Inventory _inventory;

    [SerializeField]
    private Image _traderImage;

    [SerializeField]
    private TextMeshProUGUI _traderType;

    [SerializeField]
    private TextMeshProUGUI _selectedItemName;

    [SerializeField]
    private TextMeshProUGUI _selectedItemEffect;

    [SerializeField]
    private TextMeshProUGUI _selectedItemDescription;

    [SerializeField]
    private GameObject _tradePanel;

    [SerializeField]
    private GameObject _itemDescriptionPanel;

    [SerializeField]
    private GameObject _tradeButton;

    [SerializeField]
    private GameObject _tradeUiPrefab;

    [SerializeField]
    private Transform _tradeParent;

    [SerializeField] 
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _inventoryFullText;

    [SerializeField]
    private GameObject _notEnoughResourcesText;

    private List<Trade> _trades = new();

    private Trade _selectedTrade;

    public EventSystem eventSystem;

    public static TradeManager instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _inventory = Inventory.Instance;
    }

    //configure and open the trade panel
    public void OpenTradePanel(List<TradeData> trades, Sprite traderImage, string traderType)
    {
        if (_playerInput.actions.FindActionMap("InGame").enabled)
        {
            _playerInput.actions.FindActionMap("InGame").Disable();
            _playerInput.actions.FindActionMap("Trader").Enable();

            Inventory.Instance.OpenInventory(true);
            _traderType.text = traderType;
            _traderImage.sprite = traderImage;
            foreach (var trade in trades)
            {
                GameObject tradeItemGO = Instantiate(_tradeUiPrefab, _tradeParent);
                tradeItemGO.GetComponent<Trade>().Configure(trade);
                tradeItemGO.GetComponent<Button>().onClick.AddListener(() => SelectTrade(tradeItemGO.GetComponent<Trade>()));
                _trades.Add(tradeItemGO.GetComponent<Trade>());
            }
            _tradeButton.GetComponent<Button>().interactable = false;
            _notEnoughResourcesText.SetActive(false);
            _inventoryFullText.SetActive(false);
            _itemDescriptionPanel.SetActive(false);
            _tradePanel.SetActive(true);
            eventSystem.SetSelectedGameObject(_trades[0].gameObject);
        }
    }

    //destroy all trades UI gameobjects
    private void ClearTradesGameobjects()
    {
        foreach (var var in _trades)
        {
            Destroy(var.gameObject);
        }
        _trades.Clear();
    }

    //close trade panel
    public void CloseTradePanel()
    {
        if (_playerInput.actions.FindActionMap("Trader").enabled)
        {
            _playerInput.actions.FindActionMap("Trader").Disable();
            _playerInput.actions.FindActionMap("InGame").Enable();

            Inventory.Instance.OpenInventory(false);

            ClearTradesGameobjects();
            _tradePanel.SetActive(false);
        }
    }

    //refresh the trades visuals
    private void RefreshTrades()
    {
        foreach (var trade in _trades)
        {
            trade.Refresh();
        }
    }

    //refresh the trades visuals, select a trade, update the trade button and open the description panel
    public void SelectTrade(Trade trade)
    {
        RefreshTrades();
        UpdateTradeButton(trade);
        _selectedTrade = trade;
        OpenDescriptionPanel();
    }

    //update the trade button
    private void UpdateTradeButton(Trade trade)
    {
        if (trade.Tradable.isTradable)
        {
            _tradeButton.GetComponent<Button>().interactable = true;

            _notEnoughResourcesText.SetActive(false);
            _inventoryFullText.SetActive(false);
        }
        else 
        { 
            _tradeButton.GetComponent<Button>().interactable = false;
            if (trade.Tradable.isMissingResources)
            {
                _notEnoughResourcesText.SetActive(true);
                _inventoryFullText.SetActive(false);
            }
            else if (trade.Tradable.isInventoryFull)
            {
                _inventoryFullText.SetActive(true);
                _notEnoughResourcesText.SetActive(false);
            }
        }
    }

    //do the select trade
    public void DoTheTrade()
    {
        //methode to remove items need from inventory
        for (int i = 0; i < _selectedTrade.currentData.itemsToTrade.Count; i++)
        {
            ItemWithQuantity itemToRemove = _selectedTrade.currentData.itemsToTrade[i];
            _inventory.RemoveItems(itemToRemove.item, itemToRemove.quantityNeed);
        }

        //methode to add trade item to inventory
        for(int i = 0; i < _selectedTrade.currentData.tradeItemQuantity; i++)
        {
            _inventory.AddItem(_selectedTrade.currentData.tradeItem);
        }
        
        UpdateTradeButton(_selectedTrade);
        RefreshTrades();
    }

    //configure and open the description panel
    private void OpenDescriptionPanel()
    {
        SetDescriptionPanel();
        _itemDescriptionPanel.SetActive(true);
    }

    //update the description panel
    private void SetDescriptionPanel()
    {
        _selectedItemName.text = _selectedTrade.currentData.tradeItem.Name;
        _selectedItemDescription.text = _selectedTrade.currentData.tradeItem.Description;
    }
}

[System.Serializable]
public struct TradeData
{
    public Item tradeItem;
    public int tradeItemQuantity;
    public List<ItemWithQuantity> itemsToTrade;
}

[System.Serializable]
public struct ItemWithQuantity
{
    public Item item;
    public int quantityNeed;
}