using UnityEngine;
using static QuestManager;

[CreateAssetMenu(fileName = "QuestTrigger", menuName = "ScriptableObjects/QuestAction/QuestTrigger", order = 1)]
[System.Serializable]
public class QuestTrigger : QuestAction
{
    [SerializeField]
    QuestTriggerType _questTriggerType = new();

    [SerializeField]
    private string _information;

    [SerializeField]
    private string _objectifText;

    //call when the QuestTrigger is the current QuestAction to configure it
    public override bool Configure(GameObjectsList objectsToActivateAtStart)
    {
        for (int i = 0; i < objectsToActivateAtStart.gameObjects.Count; i++)
        {
            objectsToActivateAtStart.gameObjects[i].SetActive(true);
        }
        return false;
    }

    //call when the QuestTrigger ended
    public override void OnEnd(GameObjectsList objectsToDesactivateAtTheEnd)
    {
        for(int i =0; i < objectsToDesactivateAtTheEnd.gameObjects.Count; i++)
        {
            objectsToDesactivateAtTheEnd.gameObjects[i].SetActive(false);
        }
    }

    //return the text for the objectives
    public override string GetObjectivesText()
    {
        return _objectifText;
    }

    //return if the QuestTrigger is finished
    public bool IsFinished(QuestTriggerType questTriggerType, string triggerName)
    {
        if(questTriggerType == _questTriggerType && triggerName == _information)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}