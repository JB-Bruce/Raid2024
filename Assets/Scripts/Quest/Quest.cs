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

    [SerializeField]
    private List<GameObjectsList> _objectsToActivateAtStart;

    [SerializeField]
    private List<GameObjectsList> _objectsToDesactivateAtTheEnd;

    //return the quest name
    public string GetName() {  return _name; }
    //return the quest description
    public string GetDescription() {  return _description; }

    //return the current quest action
    public QuestAction GetCurrentQuestAction()
    {
        return (_questActions[_currentQuestAction]);
    }

    //modify the current quest action
    public void SetCurrentQuestAction(int index)
    {
        _currentQuestAction = index;
    }

    //return the current quest action index
    public int GetCurrentQuestActionIndex()
    {
        return _currentQuestAction;
    }

    //return the number of quest actions in the quest
    public int GetQuestActionCount()
    {
        return _questActions.Count;
    }
    
    //get objects to activate at start of the current quest action
    public GameObjectsList GetObjectsToActivateAtStartByQuestActionIndex(int questAction)
    {
        return _objectsToActivateAtStart[questAction];
    }

    //get objects to activate at start of the current quest action
    public GameObjectsList GetObjectsToDesactivateAtTheEndOfByQuestActionIndex(int questAction)
    {
        return _objectsToDesactivateAtTheEnd[questAction];
    }

    //update the current QuestAction if is not the last of the Quest
    public bool NextQuestAction()
    {
        GetCurrentQuestAction().OnEnd(_objectsToDesactivateAtTheEnd[_currentQuestAction]);
        
        if (_questActions.Count > _currentQuestAction+1)
        {
            _currentQuestAction += 1;
            GetCurrentQuestAction().Configure(GetObjectsToActivateAtStartByQuestActionIndex(_currentQuestAction));
            if(GetCurrentQuestAction().GetGoal() != null)
            {
                MapManager.instance.SetQuestWaypoint(GetCurrentQuestAction().GetGoal());
            }
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

[System.Serializable]
public struct GameObjectsList
{
    public List<GameObject> gameObjects;
}