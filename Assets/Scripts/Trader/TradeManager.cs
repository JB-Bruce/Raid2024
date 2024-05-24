using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _traderSprite;

    [SerializeField]
    private TextMeshProUGUI _traderName;

    [SerializeField]
    private TextMeshProUGUI _selectedItemName;

    [SerializeField]
    private TextMeshProUGUI _selectedItemEffect;

    [SerializeField]
    private TextMeshProUGUI _selectedItemDescription;

    [SerializeField]
    private List<GameObject> _trades;

    public static TradeManager instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
    }

    public void OpenTradePanel(List<Trade> trades, string traderName, Sprite traderImage)
    {
        _traderName.text = traderName;

    }
}
