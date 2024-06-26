using System.Collections.Generic;
using UnityEngine;

public class PnjFactionTrader : Pnj
{
    public List<TradeData> _trades = new();

    [SerializeField]
    private string _traderType;

    public Sprite _traderImage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }

    //call the methode in trade manager to open the trade panel
    public void Trade()
    {
        TradeManager.instance.OpenTradePanel(_trades, _traderImage, _traderType);
    }

    protected override void Interact()
    {
        Trade();
    }
}