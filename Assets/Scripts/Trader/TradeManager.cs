using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    [SerializeField]
    private Image _traderImage;

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
    private GameObject _traderButton;

    [SerializeField]
    private Transform _tradeParent;

    private List<Trade> _trades = new();

    private Trade _selectedTrade;

    public static TradeManager instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
    }

    //configure and open the trade panel
    public void OpenTradePanel(List<TradeData> trades, Sprite traderImage)
    {
        _traderImage.sprite = traderImage;
        foreach (var trade in trades)
        {
            GameObject tradeItemGO = Instantiate(_tradeUiPrefab, _tradeParent);
            tradeItemGO.GetComponent<Trade>().Configure(trade);
            tradeItemGO.GetComponent<Button>().onClick.AddListener(() => SelectTrade(tradeItemGO.GetComponent<Trade>()));
            _trades.Add(tradeItemGO.GetComponent<Trade>());
        }
        _traderButton.SetActive(false);
        _tradeButton.GetComponent<Button>().interactable = false;
        _itemDescriptionPanel.SetActive(false);
        _tradePanel.SetActive(true);
    }

    //close trade panel
    public void CloseTradePanel()
    {
        _traderButton.SetActive(true);
        _tradePanel.SetActive(false);
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
        if (trade.isTradable)
        {
            _tradeButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            _tradeButton.GetComponent<Button>().interactable = false;
        }
    }

    //do the select trade
    public void DoTheTrade()
    {
        print("trade");
        //methode to remove items need
        //methode to add trade item
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
    public List<ItemToTradeData> itemsToTrade;
}

[System.Serializable]
public struct ItemToTradeData
{
    public Item tradeItem;
    public int quantityNeed;
}