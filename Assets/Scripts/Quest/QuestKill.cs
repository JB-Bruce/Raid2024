using UnityEngine;

[CreateAssetMenu(fileName = "QuestKill", menuName = "ScriptableObjects/QuestAction/QuestKill", order = 1)]
[System.Serializable]
public class QuestKill : QuestAction
{
    [SerializeField]
    Faction _faction = new();

    [SerializeField]
    private int _numberToKill;

    [SerializeField]
    private int _killCount;

    //call when the QuestKill is the current QuestAction to configure it
    public override void Configure()
    {
        _killCount = 0;
    }

    //check if the pnj killed is from the good faction to increment the count
    private void CheckKill(Faction faction)
    {
        if(faction == _faction)
        {
            _killCount++;
        }
    }

    //return if the QuestKill is finished
    public bool IsFinished(Faction faction)
    {
        CheckKill(faction);
        if (_killCount < _numberToKill)
        {
            return false;
        }
        return true;
    }
}
