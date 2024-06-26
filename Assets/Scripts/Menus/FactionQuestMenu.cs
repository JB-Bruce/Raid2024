using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionQuestMenu : MonoBehaviour
{
    [SerializeField] private Image _logo;
    [SerializeField] private TextMeshProUGUI _title;
    private PnjFactionQuestGiver _questGiver;
    [SerializeField] private Sprite[] _logoSprites = new Sprite[4];
    [SerializeField] private GameObject _quest;
    [SerializeField] private GameObject _acceptQuest;
    [SerializeField] private float _removeReputation;
    private FactionQuestManager _factionQuestManager;

    private FactionManager _factionManager;

    private void Start()
    {
        _factionQuestManager = FactionQuestManager.instance;
        _factionManager = FactionManager.Instance;
    }

    // Set All the information in the menu
    private void SetMenu()
    {
        _title.text = '"' + _questGiver.GetName + '"';
        _logo.sprite = GetLogo(_questGiver.GetFaction);

        Quest acceptedQuest = _factionQuestManager.GetQuestByFaction(_questGiver.GetFaction);
        if (acceptedQuest != null)
        {
            _quest.SetActive(false);
            _acceptQuest.SetActive(true);
            SetQuestText(_acceptQuest, acceptedQuest);
            return;
        }

        _quest.SetActive(true);
        _acceptQuest.SetActive(false);
        for (int i = 0;  i < 4; i++)
        {
            SetQuestText(_quest.transform.GetChild(i).gameObject, _factionQuestManager.GetQuest(_questGiver.FactionQuest[i]));
        }
    }

    // Set one Quest in the menu
    private void SetQuestText(GameObject questText ,Quest quest)
    {
        questText.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = quest.GetName();
        questText.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = quest.GetDescription();
    }

    // Open the faction quest Menu
    public void Open()
    {
        gameObject.SetActive(true);
        //SetMenu();
        Time.timeScale = 0;
    }

    // Close the faction quest menu
    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Accept quest
    public void AcceptQuest(int index)
    {
        _questGiver.AcceptFactionQuest(index);
        SetMenu();
    }

    // Cancel the current quest
    public void CancelQuest()
    {
        _factionQuestManager.GiveUpQuest(_questGiver.GetFaction);
        _factionManager.AddReputation(Faction.Player, _questGiver.GetFaction, -_removeReputation);
        SetMenu();
    }

    // return the sprite of the faction in entry
    private Sprite GetLogo(Faction faction)
    {
        switch (faction) 
        {
            case Faction.Utopist :
                return _logoSprites[0];
            case Faction.Scientist: 
                return _logoSprites[1];
            case Faction.Military: 
                return _logoSprites[2];
            default:
                return _logoSprites[3];
        }
    }
}
