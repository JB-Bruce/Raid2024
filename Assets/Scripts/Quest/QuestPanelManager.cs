using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainQuestPanelButton;

    [SerializeField]
    private GameObject _mainQuestPanel;

    [SerializeField]
    private TextMeshProUGUI _mainQuestTitle;

    [SerializeField]
    private TextMeshProUGUI _mainQuestDescription;

    [SerializeField]
    private TextMeshProUGUI _questActionTitle;

    [SerializeField]
    private TextMeshProUGUI _objectives;

    [SerializeField]
    private TextMeshProUGUI _questCounter;

    [SerializeField]
    private TextMeshProUGUI _questActionCounter;

    [SerializeField]
    private GameObject _questActionUi;

    [SerializeField]
    private GameObject _questEndedUi;

    private int _selectedQuestIndex;


    [SerializeField]
    private GameObject _factionQuestPanel;

    [SerializeField]
    private TextMeshProUGUI _utopistFactionQuestActionTitle;

    [SerializeField]
    private TextMeshProUGUI _utopistFactionQuestActionObjectives;

    [SerializeField]
    private GameObject _utopistFactionQuestActionUi;

    [SerializeField]
    private GameObject _utopistFactionQuestActionNoQuest;

    [SerializeField]
    private TextMeshProUGUI _militaryFactionQuestActionTitle;

    [SerializeField]
    private TextMeshProUGUI _militaryFactionQuestActionObjectives;

    [SerializeField]
    private GameObject _militaryFactionQuestActionUi;

    [SerializeField]
    private GameObject _militaryFactionQuestActionNoQuest;

    [SerializeField]
    private TextMeshProUGUI _survivalistFactionQuestActionTitle;

    [SerializeField]
    private TextMeshProUGUI _survivalistFactionQuestActionObjectives;

    [SerializeField]
    private GameObject _survivalistFactionQuestActionUi;

    [SerializeField]
    private GameObject _survivalistFactionQuestActionNoQuest;

    [SerializeField]
    private TextMeshProUGUI _scientistFactionQuestActionTitle;

    [SerializeField]
    private TextMeshProUGUI _scientistFactionQuestActionObjectives;

    [SerializeField]
    private GameObject _scientistFactionQuestActionUi;

    [SerializeField]
    private GameObject _scientistFactionQuestActionNoQuest;

    [SerializeField]
    private bool _mainQuestFinish = false;

    public EventSystem eventSystem;

    //configure quest panel
    public void ConfigurePanel()
    {
        OpenMainQuestPanel();
        eventSystem.SetSelectedGameObject(_mainQuestPanelButton);
    }

    //open main quest panel
    public void OpenMainQuestPanel() 
    {
        _selectedQuestIndex = QuestManager.instance.GetCurrentMainQuestActionIndex()[0];
        UpdateMainQuestUi();
        _factionQuestPanel.SetActive(false);
        _mainQuestPanel.SetActive(true);
    }

    //open faction quest panel
    public void OpenFactionQuestPanel()
    {
        UpdateFactionsQuestUi();
        _mainQuestPanel.SetActive(false);
        _factionQuestPanel.SetActive(true);
    }

    //update main quest ui
    private void UpdateMainQuestUi()
    {
        QuestManager questManager = QuestManager.instance;
        Quest selectedQuest = questManager.GetMainQuestByIndex(_selectedQuestIndex);
        List<int> indexs = questManager.GetCurrentMainQuestActionIndex();
        _mainQuestTitle.text = selectedQuest.GetName();
        _mainQuestDescription.text = selectedQuest.GetDescription();
        _questCounter.text = _selectedQuestIndex+1 + "/" + (indexs[0]+1);
        if (_selectedQuestIndex == indexs[0] && !_mainQuestFinish)
        {
            _questActionTitle.text = selectedQuest.GetCurrentQuestAction().GetName();
            _objectives.text = selectedQuest.GetCurrentQuestAction().GetObjectivesText();
            _questActionCounter.text = indexs[1]+1 + "/" + questManager.GetMainQuestActionCountByIndex(_selectedQuestIndex);
            _questActionUi.SetActive(true);
            _questEndedUi.SetActive(false);
        }
        else
        {
            _questEndedUi.SetActive(true);
            _questActionUi.SetActive(false);
        }

    }

    //update faction quest ui
    private void UpdateFactionsQuestUi()
    {
        FactionQuestManager factionQuestManager = FactionQuestManager.instance;

        if(factionQuestManager.GetIndexRefByFaction(Faction.Utopist) == -1)
        {
            _utopistFactionQuestActionUi.SetActive(false);
            _utopistFactionQuestActionNoQuest.SetActive(true);
        }
        else
        {
            QuestAction currentQuestAction = factionQuestManager.GetQuestByFaction(Faction.Utopist).GetCurrentQuestAction();
            _utopistFactionQuestActionTitle.text = currentQuestAction.GetName();
            _utopistFactionQuestActionObjectives.text = currentQuestAction.GetObjectivesText();
            _utopistFactionQuestActionNoQuest.SetActive(false);
            _utopistFactionQuestActionUi.SetActive(true);
        }

        if (factionQuestManager.GetIndexRefByFaction(Faction.Military) == -1)
        {
            _militaryFactionQuestActionUi.SetActive(false);
            _militaryFactionQuestActionNoQuest.SetActive(true);
        }
        else
        {
            QuestAction currentQuestAction = factionQuestManager.GetQuestByFaction(Faction.Military).GetCurrentQuestAction();
            _militaryFactionQuestActionTitle.text = currentQuestAction.GetName();
            _militaryFactionQuestActionObjectives.text = currentQuestAction.GetObjectivesText();
            _militaryFactionQuestActionNoQuest.SetActive(false);
            _militaryFactionQuestActionUi.SetActive(true);
        }

        if (factionQuestManager.GetIndexRefByFaction(Faction.Survivalist) == -1)
        {
            _survivalistFactionQuestActionUi.SetActive(false);
            _survivalistFactionQuestActionNoQuest.SetActive(true);
        }
        else
        {
            QuestAction currentQuestAction = factionQuestManager.GetQuestByFaction(Faction.Survivalist).GetCurrentQuestAction();
            _survivalistFactionQuestActionTitle.text = currentQuestAction.GetName();
            _survivalistFactionQuestActionObjectives.text = currentQuestAction.GetObjectivesText();
            _survivalistFactionQuestActionNoQuest.SetActive(false);
            _survivalistFactionQuestActionUi.SetActive(true);
        }

        if (factionQuestManager.GetIndexRefByFaction(Faction.Scientist) == -1)
        {
            _scientistFactionQuestActionUi.SetActive(false);
            _scientistFactionQuestActionNoQuest.SetActive(true);
        }
        else
        {
            QuestAction currentQuestAction = factionQuestManager.GetQuestByFaction(Faction.Scientist).GetCurrentQuestAction();
            _scientistFactionQuestActionTitle.text = currentQuestAction.GetName();
            _scientistFactionQuestActionObjectives.text = currentQuestAction.GetObjectivesText();
            _scientistFactionQuestActionNoQuest.SetActive(false);
            _scientistFactionQuestActionUi.SetActive(true);
        }
    }

    //display the next displayed quest
    public void NextMainQuest()
    {
        if (_selectedQuestIndex < QuestManager.instance.GetCurrentMainQuestActionIndex()[0])
        {
            _selectedQuestIndex += 1;
        }
        UpdateMainQuestUi();
    }

    //display the previous displayed quest
    public void PreviousMainQuest()
    {
        if (_selectedQuestIndex > 0)
        {
            _selectedQuestIndex -= 1;
        }
        UpdateMainQuestUi();
    }

    //set the bool main quest finish to true
    public void EndMainQuest()
    {
        _mainQuestFinish = true;
    }
}