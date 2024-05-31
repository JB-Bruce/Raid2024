using System.Collections.Generic;
using UnityEngine;

public class PnjFactionTrader : MonoBehaviour
{
    [SerializeField]
    private List<TradeData> _trades = new();

    [SerializeField]
    private string _traderName;

    [SerializeField]
    private Sprite _traderImage;

    //call the methode in trade manager to open the trade panel
    public void Trade()
    {
        TradeManager.instance.OpenTradePanel(_trades, _traderImage);
    }
}