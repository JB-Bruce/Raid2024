using UnityEngine;

[System.Serializable]
public abstract class QuestAction : ScriptableObject
{
    [SerializeField]
    private string _description;

    //call when the QuestAction is the current QuestAction to configure it
    public abstract void Configure();
}