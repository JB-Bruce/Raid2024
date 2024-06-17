using System.Collections.Generic;
using UnityEngine;
using static QuestManager;

public class FactionQuestManager : MonoBehaviour
{
    [SerializeField]
    private int _currentUtopistFactionQuest;

    [SerializeField]
    private int _currentMilitaryFactionQuest;

    [SerializeField]
    private int _currentSurvivalistFactionQuest;

    [SerializeField]
    private int _currentIntellectualFactionQuest;

    [SerializeField]
    private List<Quest> _factionQuests = new();

    public static FactionQuestManager instance;

    //create an instance of the FactionQuestManager
    private void Awake()
    {
        instance = this;
    }

    //set the currents factions quests index
    private void Start()
    {
        _currentUtopistFactionQuest = -1;
        _currentMilitaryFactionQuest = -1;
        _currentSurvivalistFactionQuest = -1;
        _currentIntellectualFactionQuest = -1;
    }

    //check if the currents FactionQuests are QuestsTrigger
    public void CheckFactionQuestsTrigger(QuestTriggerType questTrigger, string information = "")
    {
        CheckFactionQuestTrigger(questTrigger, ref _currentUtopistFactionQuest, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentMilitaryFactionQuest, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentSurvivalistFactionQuest, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentIntellectualFactionQuest, information);
    }

    //check if the current FactionQuestAction is a QuestTrigger
    private void CheckFactionQuestTrigger(QuestTriggerType questTrigger, ref int questIndex, string information = "")
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestTrigger aQuestTrigger)
        {
            if (aQuestTrigger.IsFinished(questTrigger, information))
            {
                NextQuest(ref questIndex);
            }
        }
    }

    //check if the currents FactionQuests are QuestsItems
    public void CheckFactionQuestsItems(ItemWithQuantity itemWithQuantity)
    {
        CheckFactionQuestItems(itemWithQuantity, ref _currentUtopistFactionQuest);
        CheckFactionQuestItems(itemWithQuantity, ref _currentMilitaryFactionQuest);
        CheckFactionQuestItems(itemWithQuantity, ref _currentSurvivalistFactionQuest);
        CheckFactionQuestItems(itemWithQuantity, ref _currentIntellectualFactionQuest);
    }

    //check if the current FactionQuestAction is a QuestItems
    private void CheckFactionQuestItems(ItemWithQuantity itemWithQuantity, ref int questIndex)
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestItems aQuestItems)
        {
            if (aQuestItems.IsFinished(itemWithQuantity))
            {
                NextQuest(ref questIndex);
            }
        }
    }

    //check if the currents FactionQuests are QuestsKill
    public void CheckFactionQuestsKill(Faction faction, string enemyType)
    {
        CheckFactionQuestKill(faction, ref _currentUtopistFactionQuest, enemyType);
        CheckFactionQuestKill(faction, ref _currentMilitaryFactionQuest, enemyType);
        CheckFactionQuestKill(faction, ref _currentSurvivalistFactionQuest, enemyType);
        CheckFactionQuestKill(faction, ref _currentIntellectualFactionQuest, enemyType);
    }

    //check if the current FactionQuestAction is a QuestKill
    private void CheckFactionQuestKill(Faction faction, ref int questIndex, string enemyType)
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestKill aQuestKill)
        {
            if (aQuestKill.IsFinished(faction, enemyType))
            {
                NextQuest(ref questIndex);
            }
        }
    }

    //select a faction quest
    public void SelectFactionQuest(int indexQuest, string faction)
    {
        GetIndexRefByFaction(faction) = indexQuest;
    }

    //return the ref of a faction quest index with the faction name
    public ref int GetIndexRefByFaction(string faction)
    {
        if(faction == "Utopist")
        {
            return ref _currentUtopistFactionQuest;
        }
        else if (faction == "Military") 
        {
            return ref _currentMilitaryFactionQuest;
        }
        else if (faction == "Survivalist")
        {
            return ref _currentSurvivalistFactionQuest;
        }
        else
        {
            return ref _currentIntellectualFactionQuest;
        }
    }

    //update the current Quest to null if the current QuestAction is the last of the Quest
    private void NextQuest(ref int questIndex)
    {
        if (!_factionQuests[questIndex].NextQuestAction())
        {
            questIndex = -1;
        }
    }

    //return the number of various factionQuest
    public int GetFactionQuestQuantity()
    {
        return _factionQuests.Count;
    }

    //give up the quest of a faction
    public void GiveUpQuest(string faction)
    {
        GetIndexRefByFaction(faction) = -1;
    }

    //return the quest of a faction
    public Quest GetQuestByFaction(string faction)
    {
        return _factionQuests[GetIndexRefByFaction(faction)];
    }
}