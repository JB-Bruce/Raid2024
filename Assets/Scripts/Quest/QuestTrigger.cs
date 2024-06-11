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

    //call when the QuestTrigger is the current QuestAction to configure it
    public override bool Configure(){ return false; }

    //return the text for the objectives
    public override string GetObjectivesText()
    {
        return "- Aller parler à " + _information;
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
