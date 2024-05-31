using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private int _currentMainQuest;

    [SerializeField]
    private List<Quest> _quests = new();

    public static QuestManager instance;

    [SerializeField]
    private Item _bandage;

    [SerializeField]
    private Item _alcohol;

    //create an instance of the DialogueManager
    private void Awake()
    {
        instance = this;
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
    }

    //get the current main quest action index
    public List<int> GetCurrentMainQuestActionIndex()
    {
        List<int> indexs = new ();
        indexs.Add (_currentMainQuest);
        indexs.Add (_quests[_currentMainQuest].GetCurrentQuestActionIndex());
        return indexs;
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