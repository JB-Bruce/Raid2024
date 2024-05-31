using UnityEngine;
using static QuestManager;

[CreateAssetMenu(fileName = "QuestTrigger", menuName = "ScriptableObjects/QuestAction/QuestTrigger", order = 1)]
[System.Serializable]
public class QuestTrigger : QuestAction
{
    [SerializeField]
    questTriggerType _questTriggerType = new();

    [SerializeField]
    private string _information;

    //call when the QuestTrigger is the current QuestAction to configure it
    public override void Configure(){}

    //return if the QuestTrigger is finished
    public bool IsFinished(questTriggerType questTriggerType, string triggerName)
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
