using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _questPanel;

    [SerializeField]
    private GameObject _questInfoInGame;

    [SerializeField]
    private GameObject _closeButton;

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

    public EventSystem eventSystem;

    [SerializeField]
    private List<Quest> _quests = new();

    public static QuestManager instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
        UpdateObjectivesUi();
        UpdateQuestActionUi();
    }

    //check if the current QuestAction is a QuestTrigger
    public void CheckQuestTrigger(questTriggerType questTrigger, string information = "")
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestTrigger aQuestTrigger)
        {
            if (aQuestTrigger.IsFinished(questTrigger, information))
            {
                NextMainQuest();
            }
            UpdateObjectivesUi();
        }
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
        eventSystem.SetSelectedGameObject(_closeButton);
        if (_questInfoInGame.activeInHierarchy)
        {
            UpdateMainQuestUi();
            UpdateQuestActionUi();
            _questInfoInGame.SetActive(false);
            _questPanel.SetActive(true);
        }
        else
        {
            CloseQuestPanel();
        }
    }

    //close quest panel
    public void CloseQuestPanel()
    {
        _questPanel.SetActive(false);
        _questInfoInGame.SetActive(true);
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