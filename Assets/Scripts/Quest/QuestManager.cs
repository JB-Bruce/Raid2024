using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private int _banditsAtConstructionArea;

    [SerializeField]
    private GameObject _case;

    [SerializeField]
    private GameObject _constructionArea;

    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _questPanel;

    [SerializeField]
    private TextMeshProUGUI _questActionTitleInGame;

    [SerializeField]
    private TextMeshProUGUI _objectivesInGame;

    [SerializeField]
    private int _currentMainQuest;

    public EventSystem eventSystem;

    [SerializeField]
    private List<Quest> _quests = new();

    [SerializeField]
    private FactionQuestManager _factionQuestManager;

    [SerializeField]
    private QuestPanelManager _questPanelManager;

    [SerializeField]
    private FactionUnitManager _factionUnitManager;

    public static QuestManager instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
        UpdateInGameQuestUi();
    }

    //check if the current QuestActions are QuestTrigger
    public void CheckQuestTrigger(QuestTriggerType questTrigger, string information = "")
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestTrigger aQuestTrigger)
        {
            if (aQuestTrigger.IsFinished(questTrigger, information))
            {
                NextMainQuest();
            }
        }
        _factionQuestManager.CheckFactionQuestsTrigger(questTrigger, information);
        UpdateInGameQuestUi();
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
            UpdateInGameQuestUi();
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
            UpdateInGameQuestUi();
        }
    }

    //update the current Quest if the current QuestAction is the last of the Quest
    private void NextMainQuest()
    {
        if (!_quests[_currentMainQuest].NextQuestAction())
        {
            _currentMainQuest += 1;
            _quests[_currentMainQuest].GetCurrentQuestAction().Configure(_quests[_currentMainQuest].GetObjectsToActivateAtStartOfTheCUrrentQuestAction());
        }
        List<string> actionsToDo = _quests[_currentMainQuest].GetCurrentQuestAction().GetActionsToDoAtStart();
        if (actionsToDo.Count >0)
        {
            for (int i = 0; i < actionsToDo.Count; i++)
            {
                switch (actionsToDo[i])
                {
                    case "CaseSpawnBandit":
                        _factionUnitManager.SpawnWaveUnit(_case.transform.position, _case.transform.position, 5);
                        break;
                    case "ConstructionSpawnBandit":
                        for (int j = 0; j < _banditsAtConstructionArea; j++)
                        {
                            _factionUnitManager.SpawnWaveUnit(_constructionArea.transform.position, _case.transform.position, 8);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        UpdateInGameQuestUi();
    }

    //return the current main quest action index
    public List<int> GetCurrentMainQuestActionIndex()
    {
        List<int> indexs = new()
        {
            _currentMainQuest,
            _quests[_currentMainQuest].GetCurrentQuestActionIndex()
        };
        return indexs;
    }

    //return the number of quest actions of a quest by index
    public int GetMainQuestActionCountByIndex(int questIndex)
    {
        return _quests[questIndex].GetQuestActionCount();
    }

    //open panel if he's closed and close it if he's open
    public void SetActiveQuestPanel()
    {
        if (_playerInput.actions.FindActionMap("InGame").enabled)
        {
            OpenQuestPanel();
        }
        else if (_playerInput.actions.FindActionMap("Quest").enabled)
        {
            CloseQuestPanel();
        }
    }

    //open quest panel
    public void OpenQuestPanel()
    {
        _playerInput.SwitchCurrentActionMap("Quest");
        _questPanelManager.ConfigurePanel();
        _questPanel.SetActive(true);
    }

    //close quest panel
    public void CloseQuestPanel()
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        _questPanel.SetActive(false);
    }

    //update in game quest ui
    private void UpdateInGameQuestUi()
    {
        string objectives = _quests[_currentMainQuest].GetCurrentQuestAction().GetObjectivesText();
        _objectivesInGame.text = objectives;
        string text = _quests[_currentMainQuest].GetCurrentQuestAction().GetName();
        _questActionTitleInGame.text = text;
    }

    //return the current main quest
    public Quest GetMainQuestByIndex(int index)
    {
        return _quests[index];
    }

    public enum QuestTriggerType
    {
        defend,
        enterArea,
        exitArea,
        interact,
        dialogue
    }
}