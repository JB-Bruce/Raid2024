using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _questPanel;

    [SerializeField]
    private GameObject _buttons;

    [SerializeField]
    private TextMeshProUGUI _mainQuestTitle;

    [SerializeField]
    private TextMeshProUGUI _mainQuestDescription;

    [SerializeField]
    private TextMeshProUGUI _questActionTitle;

    [SerializeField]
    private TextMeshProUGUI _objectives;

    [SerializeField]
    private TextMeshProUGUI _questActionTitleInGame;

    [SerializeField]
    private TextMeshProUGUI _objectivesInGame;

    [SerializeField]
    private int _currentMainQuest;

    [SerializeField]
    private List<Quest> _quests = new();

    [SerializeField]
    private FactionQuestManager _factionQuestManager;

    public static QuestManager instance;

    [SerializeField]
    private Item _bandage;

    [SerializeField]
    private Item _alcohol;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
        UpdateObjectivesUi();
        UpdateQuestActionUi();
    }

    //check if the current QuestActions are QuestTrigger
    public void CheckQuestTrigger(questTriggerType questTrigger, string information = "")
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestTrigger aQuestTrigger)
        {
            if (aQuestTrigger.IsFinished(questTrigger, information))
            {
                NextMainQuest();
            }
        }
        _factionQuestManager.CheckFactionQuestsTrigger(questTrigger, information);
        UpdateObjectivesUi();
    }

    //check if the current QuestAction is a QuestItems
    public void CheckQuestItems(ItemWithQuantity itemWithQuantity)
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestItems aQuestItems)
        {
            if (aQuestItems.IsFinished(itemWithQuantity))
            {
                NextMainQuest();
            }
            _factionQuestManager.CheckFactionQuestsItems(itemWithQuantity);
            UpdateObjectivesUi();
        }
    }

    //check if the current QuestAction is a QuestKill
    public void CheckQuestKill(Faction faction)
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestKill aQuestKill)
        {
            if (aQuestKill.IsFinished(faction))
            {
                NextMainQuest();
            }
            _factionQuestManager.CheckFactionQuestsKill(faction);
            UpdateObjectivesUi();
        }
    }

    //update the current Quest if the current QuestAction is the last of the Quest
    private void NextMainQuest()
    {
        if (!_quests[_currentMainQuest].NextQuestAction())
        {
            _currentMainQuest += 1;
        }
        _quests[_currentMainQuest].GetCurrentQuestAction().Configure();
        UpdateMainQuestUi();
        UpdateQuestActionUi();
    }

    //get the current main quest action index
    public List<int> GetCurrentMainQuestActionIndex()
    {
        List<int> indexs = new ();
        indexs.Add (_currentMainQuest);
        indexs.Add (_quests[_currentMainQuest].GetCurrentQuestActionIndex());
        return indexs;
    }

    //open quest panel
    public void OpenQuestPanel()
    {
        UpdateMainQuestUi();
        UpdateQuestActionUi();
        _buttons.SetActive(false);
        _questPanel.SetActive(true);
    }

    //close quest panel
    public void CloseQuestPanel()
    {
        _questPanel.SetActive(false);
        _buttons.SetActive(true);
    }

    //update main quest ui
    private void UpdateMainQuestUi()
    {
        _mainQuestTitle.text = _quests[_currentMainQuest].GetName();
        _mainQuestDescription.text = _quests[_currentMainQuest].GetDescription();
    }

    //update quest action ui
    private void UpdateQuestActionUi()
    {
        string text = _quests[_currentMainQuest].GetCurrentQuestAction().GetName();
        _questActionTitleInGame.text = text;
        _questActionTitle.text = text;
    }

    //update quest objectives informations ui
    private void UpdateObjectivesUi()
    {
        string objectives = _quests[_currentMainQuest].GetCurrentQuestAction().GetObjectivesText();
        _objectivesInGame.text = objectives;
        _objectives.text = objectives;
    }

    public void TuerUnMillitaire() { CheckQuestKill(Faction.Military); }
    public void TuerUnUtopiste() { CheckQuestKill(Faction.Utopist); }
    public void AjouterUnBandage() 
    {
        ItemWithQuantity item = new();
        item.item = _bandage;
        item.quantityNeed = 1;
        CheckQuestItems(item);
    }

    public void RetirerDeuxBandage()
    {
        ItemWithQuantity item = new();
        item.item = _bandage;
        item.quantityNeed = -2;
        CheckQuestItems(item);
    }

    public void AjouterUnAlcohol()
    {
        ItemWithQuantity item = new();
        item.item = _alcohol;
        item.quantityNeed = 1;
        CheckQuestItems(item);
    }

    public enum questTriggerType
    {
        defend,
        enterArea,
        buildCoil,
        plantingDynamiteInTheBanditCamp,
        readRansomPaper,
        fillTheEngine,
        startMagneticCoil,
        dialogue
    }
}