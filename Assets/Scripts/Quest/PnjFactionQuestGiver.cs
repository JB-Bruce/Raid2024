using UnityEngine;

public class PnjFactionQuestGiver : Pnj
{
    [SerializeField]
    private string _factionName;

    [SerializeField]
    private static int _giverQuestQuantity;

    private static int _factionQuestsQuantity;

    [SerializeField]
    private int[] _factionQuest = new int[_giverQuestQuantity];

    private void Start()
    {
        _factionQuestsQuantity = FactionQuestManager.instance.GetFactionQuestQuantity();
        Configure();
    }

    //configure the list of quests proposed by the factionQuest giver
    private void Configure()
    {
        for (int i = 0; i < _giverQuestQuantity; i++)
        {
            DefineQuest(i);
        }
    }

    //define a quest in the list of quests proposed by the factionQuest giver
    private void DefineQuest(int index)
    {
        int previousQuest = _factionQuest[index];
        int nextQuest = Random.Range(0, _factionQuestsQuantity);
        while(nextQuest ==  previousQuest)
        {
            nextQuest = Random.Range(0, _factionQuestsQuantity);
        }
        _factionQuest[index] = nextQuest;
    }

    //accept a faction quest
    public void AcceptFactionQuest(int indexQuestInList)
    {
        FactionQuestManager.instance.SelectFactionQuest(_factionQuest[indexQuestInList], _factionName);
        DefineQuest(indexQuestInList);
    }
}