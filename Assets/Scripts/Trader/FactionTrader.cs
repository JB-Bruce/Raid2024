using System.Collections.Generic;
using UnityEngine;

public class FactionTrader : MonoBehaviour
{
    private List<Trade> _trades;

    [SerializeField]
    private string _traderName;

    [SerializeField]
    private Sprite _traderImage;

    public void Trade()
    {
        TradeManager.instance.OpenTradePanel(_trades, _traderName, _traderImage);
    }
}

[System.Serializable]
public struct Trade
{
    public ScriptableObject tradeItem;
    public List<ItemToTrade> itemsToTrade;
}

[System.Serializable]
public struct ItemToTrade
{
    public ScriptableObject tradeItem;
    public int quantityNeed;
}