using UnityEngine;

[CreateAssetMenu(fileName = "QuestKill", menuName = "ScriptableObjects/QuestAction/QuestKill", order = 1)]
[System.Serializable]
public class QuestKill : QuestAction
{
    [SerializeField]
    Faction _faction = new();

    [SerializeField]
    private string _enemyType;

    [SerializeField]
    private int _numberToKill;

    [SerializeField]
    private int _killCount;

    //call when the QuestKill is the current QuestAction to configure it
    public override bool Configure(GameObjectsList objectsToActivateAtStart)
    {
        _killCount = 0;
        for (int i = 0; i < objectsToActivateAtStart.gameObjects.Count; i++)
        {
            objectsToActivateAtStart.gameObjects[i].SetActive(true);
        }
        return false;
    }

    //call when the QuestKill ended
    public override void OnEnd(GameObjectsList objectsToDesactivateAtTheEnd) { }

    //return the text for the objectives
    public override string GetObjectivesText()
    {
        return "- Tuer des " + _faction + "   " + _killCount + "/" + _numberToKill + "\n";
    }

    //check if the pnj killed is from the good faction to increment the count
    private void CheckKill(Faction faction, string enemyType)
    {
        if(faction == _faction && enemyType == _enemyType)
        {
            _killCount++;
        }
    }

    //return if the QuestKill is finished
    public bool IsFinished(Faction faction, string enemyType)
    {
        CheckKill(faction, enemyType);
        if (_killCount < _numberToKill)
        {
            return false;
        }
        return true;
    }
}