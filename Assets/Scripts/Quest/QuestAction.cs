using UnityEngine;

[System.Serializable]
public abstract class QuestAction : ScriptableObject
{
    [SerializeField]
    private string _name;

    public string GetName() { return _name; }

    //call when the QuestAction is the current QuestAction to configure it
    public abstract void Configure();

    //return the objectives text
    public abstract string GetObjectivesText();
}