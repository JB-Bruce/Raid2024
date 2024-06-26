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
    private ItemWithQuantity _checkItem;

    [SerializeField]
    private Item _dynamite;

    [SerializeField]
    private Item _steelPlate;

    [SerializeField]
    private Item _engine;

    [SerializeField]
    private Item _ferriteCore;

    [SerializeField]
    private Item _plastic;

    [SerializeField]
    private Item _metal;

    [SerializeField]
    private GameObject _case;
    
    [SerializeField]
    private GameObject _bruce;

    [SerializeField]
    private GameObject _bruceTpPosition;

    [SerializeField]
    private GameObject _bruceTpBackPosition;

    [SerializeField]
    private GameObject _constructionSpot;

    [SerializeField]
    private GameObject _objectToDefend;

    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _endMainQuestPanel;

    [SerializeField]
    private GameObject _questPanel;

    [SerializeField]
    private GameObject _questInfosInGame;

    [SerializeField]
    private TextMeshProUGUI _questActionTitleInGame;

    [SerializeField]
    private TextMeshProUGUI _objectivesInGame;

    [SerializeField]
    private int _currentMainQuest;

    [SerializeField]
    private QuestAction _utopistFisrtMainQuest;

    [SerializeField]
    private QuestAction _survivalistFisrtMainQuest;

    [SerializeField]
    private QuestAction _scientistFisrtMainQuest;

    [SerializeField]
    private QuestAction _militaryFisrtMainQuest;

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

    private void Start()
    {
        string factionChoose = PlayerPrefs.GetString("ChooseFaction");
        if (factionChoose == "Survivalist")
        {
            _quests[0].SetQuestAction(0, _survivalistFisrtMainQuest);
        }
        else if (factionChoose == "Utopist")
        {
            _quests[0].SetQuestAction(0, _utopistFisrtMainQuest);
        }
        else if (factionChoose == "Scientist")
        {
            _quests[0].SetQuestAction(0, _scientistFisrtMainQuest);
        }
        else if (factionChoose == "Military")
        {
            _quests[0].SetQuestAction(0, _militaryFisrtMainQuest);
        }
        MapManager.instance.SetQuestWaypoint(_quests[_currentMainQuest].GetCurrentQuestAction().GetGoal());
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

    //check if the current QuestAction is a QuestPick
    public void CheckQuestPick(float quantityPick, string stuffToPick)
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestPick aQuestPick)
        {
            if (aQuestPick.IsFinished(quantityPick, stuffToPick))
            {
                NextMainQuest();
            }
            _factionQuestManager.CheckFactionQuestsPick(quantityPick, stuffToPick);
            UpdateInGameQuestUi();
        }
    }

    //check if the current QuestAction is a QuestKill
    public void CheckQuestKill(Faction faction, string enemyType = "")
    {
        if (_quests[_currentMainQuest].GetCurrentQuestAction() is QuestKill aQuestKill)
        {
            if (aQuestKill.IsFinished(faction, enemyType))
            {
                NextMainQuest();
            }
            _factionQuestManager.CheckFactionQuestsKill(faction, enemyType);
            UpdateInGameQuestUi();
        }
    }

    //update the current Quest if the current QuestAction is the last of the Quest
    private void NextMainQuest()
    {
        if (!_quests[_currentMainQuest].NextQuestAction())
        {
            if(_currentMainQuest+1 < _quests.Count)
            {
                _currentMainQuest += 1;
                if (QuestAchievedPanelManager.Instance)
                {
                    QuestAchievedPanelManager.Instance.PlayQuestAchievedAnimation();
                    SoundManager.instance.PlaySFX("QuestAchieved", SoundManager.instance._sfxPlayer);
                }
            }
            else
            {
                _questPanelManager.EndMainQuest();
                EndMainQuests();
            }
            
            Quest currentMainQuest = _quests[_currentMainQuest];
            currentMainQuest.GetCurrentQuestAction().Configure(currentMainQuest.GetObjectsToActivateAtStartByQuestActionIndex(currentMainQuest.GetCurrentQuestActionIndex()));
            if(currentMainQuest.GetCurrentQuestAction().GetGoal() != Vector3.zero)
            {
                MapManager.instance.SetQuestWaypoint(currentMainQuest.GetCurrentQuestAction().GetGoal());
            }
        }
        List<string> actionsToDo = _quests[_currentMainQuest].GetCurrentQuestAction().GetActionsToDoAtStart();
        if (actionsToDo.Count >0)
        {
            for (int i = 0; i < actionsToDo.Count; i++)
            {
                switch (actionsToDo[i])
                {
                    case "CaseSpawnBandit":
                        _factionUnitManager.SpawnWaveUnit(_case.transform.position + new Vector3(0, 2, 0), _case.transform.position, 5);
                        break;
                    case "ConstructionSpawnBandit":
                        for (int j = 0; j < _banditsAtConstructionArea; j++)
                        {
                            _factionUnitManager.SpawnWaveUnit(_constructionSpot.transform.position + new Vector3(0, 2, 0), _constructionSpot.transform.position, 5, "clearConstructionArea");
                        }
                        break;
                    case "DefendBase":
                        Humanoid humanoide = _objectToDefend.GetComponent<Humanoid>();
                        humanoide.life = 100;
                        humanoide.SetSlider();
                        WaveManager.instance.StartWave(_constructionSpot.transform.position, 10, 1, 3);
                        break;
                    case "TpBruce":
                        _bruce.transform.position = _bruceTpPosition.transform.position;
                        break;
                    case "TpBackBruce":
                        _bruce.transform.position = _bruceTpBackPosition.transform.position;
                        break;
                    default:
                        break;
                }
            }
        }
        CheckQuestItems(_checkItem);
        UpdateInGameQuestUi();
    }

    //set the quest system on death
    public void OnDeath()
    {
        QuestAction currentQuestAction = _quests[_currentMainQuest].GetCurrentQuestAction();
        if (currentQuestAction is QuestItems questItems)
        {
            questItems.Configure(_quests[_currentMainQuest].GetObjectsToActivateAtStartByQuestActionIndex(_quests[_currentMainQuest].GetCurrentQuestActionIndex()));
        }
        else if (currentQuestAction is QuestTrigger questTrigger)
        {
            QuestTriggerType questTriggerType = questTrigger.GetQuestTriggerType();
            if (questTriggerType == QuestTriggerType.defend || questTriggerType == QuestTriggerType.exitArea) 
            {
                GameObjectsList objectsToDesactivate = _quests[_currentMainQuest].GetObjectsToActivateAtStartByQuestActionIndex(_quests[_currentMainQuest].GetCurrentQuestActionIndex());
                for (int i = 0; i < objectsToDesactivate.gameObjects.Count; i++)
                {
                    objectsToDesactivate.gameObjects[i].SetActive(false);
                }
                GameObjectsList objectsToActivate = _quests[_currentMainQuest].GetObjectsToDesactivateAtTheEndOfByQuestActionIndex(_quests[_currentMainQuest].GetCurrentQuestActionIndex() - 1);
                for (int i = 0; i < objectsToActivate.gameObjects.Count; i++)
                {
                    objectsToActivate.gameObjects[i].SetActive(true);
                }

                if (questTriggerType == QuestTriggerType.defend)
                {
                    switch (_currentMainQuest)
                    {
                        case 6:
                            Inventory.Instance.AddItem(_steelPlate, 1);
                            break;
                        case 9:
                            Inventory.Instance.AddItem(_engine, 1);
                            break;
                        case 15:
                            Inventory.Instance.AddItem(_ferriteCore, 1);
                            Inventory.Instance.AddItem(_plastic, 4);
                            Inventory.Instance.AddItem(_metal, 3);
                            break;
                        default:
                            break;
                    }
                }
                else if (questTriggerType == QuestTriggerType.exitArea)
                {
                    switch (_currentMainQuest)
                    {
                        case 11:
                            Inventory.Instance.AddItem(_dynamite, 1);
                            break;
                        default:
                            break;
                    }
                }
                _quests[_currentMainQuest].SetCurrentQuestAction(_quests[_currentMainQuest].GetCurrentQuestActionIndex() - 1);
                if (_quests[_currentMainQuest].GetCurrentQuestAction().GetGoal() != Vector3.zero)
                {
                    MapManager.instance.SetQuestWaypoint(_quests[_currentMainQuest].GetCurrentQuestAction().GetGoal());
                }
                UpdateInGameQuestUi();
            }
        }
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

    //call when all main quests are done
    private void EndMainQuests()
    {
        _playerInput.SwitchCurrentActionMap("EndGameMenu");
        Time.timeScale = 0.0f;
        _questInfosInGame.SetActive(false);
        _endMainQuestPanel.SetActive(true);
    }

    //call when the player select the button to continue to play
    public void ContinueToPlay()
    {
        _playerInput.SwitchCurrentActionMap("InGame");
        Time.timeScale = 1.0f;
        _endMainQuestPanel.SetActive(false);
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