using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //methode called by pnj to strat a dialogue
    public void StratDialogue(DialogueContent dialogue, Action<string> returnMethode)
    {

    }
}


[System.Serializable]
public struct DialogueContent
{
    public string name;
    public string description;
    public List<DialogueContent> dialogues;
    public List<string> choices;
}
