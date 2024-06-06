using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private int _currentQuestAction;

    [SerializeField]
    private string _description;

    [SerializeField]
    private List<QuestAction> _questActions = new();

    [SerializeField]
    private questType _questType;

    //get the quest name
    public string GetName() {  return _name; }
    //get the quest description
    public string GetDescription() {  return _description; }

    //get the current quest action
    public QuestAction GetCurrentQuestAction()
    {
        return (_questActions[_currentQuestAction]);
    }

    //get the current quest action index
    public int GetCurrentQuestActionIndex()
    {
        return _currentQuestAction;
    }

    //update the current QuestAction if is not the last of the Quest
    public bool NextQuestAction()
    {
        if (_questActions.Count > _currentQuestAction+1)
        {
            _currentQuestAction += 1;
            return true;
        }
        else
        {
            _currentQuestAction = 0;
            return false;
        }
    }

    public enum questType
    {
        mainQuest,
        factionQuest
    }
}