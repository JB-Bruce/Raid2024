using UnityEngine;

[System.Serializable]
public abstract class QuestAction : ScriptableObject
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private string _description;

    //get the questAction name
    public string GetName() { return _name; }
    //get the questAction description
    public string GetDescription() { return _description; }

    //call when the QuestAction is the current QuestAction to configure it
    public abstract void Configure();

    //return the objectives text
    public abstract string GetObjectivesText();
}