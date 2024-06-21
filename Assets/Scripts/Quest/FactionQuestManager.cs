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

    private FactionManager _factionManager;

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
        _factionManager = FactionManager.Instance;
    }

    //check if the currents FactionQuests are QuestsTrigger
    public void CheckFactionQuestsTrigger(QuestTriggerType questTrigger, string information = "")
    {
        CheckFactionQuestTrigger(questTrigger, ref _currentUtopistFactionQuest, Faction.Utopist, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentMilitaryFactionQuest,Faction.Military, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentSurvivalistFactionQuest, Faction.Survivalist, information);
        CheckFactionQuestTrigger(questTrigger, ref _currentIntellectualFactionQuest, Faction.Scientist, information);
    }

    //check if the current FactionQuestAction is a QuestTrigger
    private void CheckFactionQuestTrigger(QuestTriggerType questTrigger, ref int questIndex, Faction faction, string information = "")
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestTrigger aQuestTrigger)
        {
            if (aQuestTrigger.IsFinished(questTrigger, information))
            {
                GetReward(faction);
                NextQuest(ref questIndex);
            }
        }
    }

    //check if the currents FactionQuests are QuestsItems
    public void CheckFactionQuestsItems(ItemWithQuantity itemWithQuantity)
    {
        CheckFactionQuestItems(itemWithQuantity, ref _currentUtopistFactionQuest, Faction.Utopist);
        CheckFactionQuestItems(itemWithQuantity, ref _currentMilitaryFactionQuest, Faction.Military);
        CheckFactionQuestItems(itemWithQuantity, ref _currentSurvivalistFactionQuest, Faction.Survivalist);
        CheckFactionQuestItems(itemWithQuantity, ref _currentIntellectualFactionQuest, Faction.Scientist);
    }

    //check if the current FactionQuestAction is a QuestItems
    private void CheckFactionQuestItems(ItemWithQuantity itemWithQuantity, ref int questIndex, Faction faction)
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestItems aQuestItems)
        {
            if (aQuestItems.IsFinished(itemWithQuantity))
            {
                GetReward(faction);
                NextQuest(ref questIndex);
            }
        }
    }

    // Get reward for completing the quest // TODO Change value with modifiable value
    private void GetReward(Faction faction)
    {
        FactionSc _factionSc = _factionManager.GetFaction(faction);
        _factionManager.AddReputation(faction, Faction.Player, 1);
        _factionSc.FactionLeader.BuildingXP += 50;
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
                GetReward(faction);
                NextQuest(ref questIndex);
            }
        }
    }

    //check if the currents FactionQuests are QuestsPick
    public void CheckFactionQuestsPick(float quantityPick, string stuffToPick)
    {
        CheckFactionQuestPick(quantityPick, ref _currentUtopistFactionQuest, stuffToPick, Faction.Utopist);
        CheckFactionQuestPick(quantityPick, ref _currentMilitaryFactionQuest, stuffToPick, Faction.Military);
        CheckFactionQuestPick(quantityPick, ref _currentSurvivalistFactionQuest, stuffToPick, Faction.Survivalist);
        CheckFactionQuestPick(quantityPick, ref _currentIntellectualFactionQuest, stuffToPick, Faction.Scientist);
    }

    //check if the current FactionQuestAction is a QuestKill
    private void CheckFactionQuestPick(float quantityPick, ref int questIndex, string stuffToPick, Faction faction)
    {
        if (questIndex >= 0 && _factionQuests[questIndex].GetCurrentQuestAction() is QuestPick aQuestPick)
        {
            if (aQuestPick.IsFinished(quantityPick, stuffToPick))
            {
                GetReward(faction);
                NextQuest(ref questIndex);
            }
        }
    }

    //select a faction quest
    public void SelectFactionQuest(int indexQuest, Faction faction)
    {
        GetIndexRefByFaction(faction) = indexQuest;
    }

    //return the ref of a faction quest index with the faction name
    public ref int GetIndexRefByFaction(Faction faction)
    {
        switch (faction)
        {
            case Faction.Utopist:
                return ref _currentUtopistFactionQuest;

            case Faction.Military:
                return ref _currentMilitaryFactionQuest;

            case Faction.Survivalist:
                return ref _currentSurvivalistFactionQuest;

            default:
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
    public void GiveUpQuest(Faction faction)
    {
        GetIndexRefByFaction(faction) = -1;
    }

    //return the quest of a faction
    public Quest GetQuestByFaction(Faction faction)
    {
        int index = GetIndexRefByFaction(faction);

        if(index == -1)
        {
            return null;
        }

        return _factionQuests[index];
    }

    // Get the quest at this index
    public Quest GetQuest(int index)
    {
        return _factionQuests[index];
    }
}