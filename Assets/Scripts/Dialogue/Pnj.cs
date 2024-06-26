using System.Collections.Generic;
using UnityEngine;

public class Pnj : Interactable
{
    [SerializeField]
    private GameObject _highlightSprite;

    public List<DialogueContent> _dialogues = new();

    public List<QuestDialogue> _questDialogues = new();

    public DialogueContent _defaultDialogue = new();

    public Sprite _pnjSprite;

    public string _name;

    [SerializeField]
    private string _hideName;

    private bool _isNameHide;

    public void Start()
    {
        _isNameHide = true;
    }

    public override void TriggerEnter(bool state)
    {
        if (state)
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
        else
        {
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }


    public override void Highlight(bool state)
    {
        if (_highlightSprite != null)
        {
            _highlightSprite.SetActive(state);
        }
    }

    //call the DialogueManager to start the dialogue
    public void StartDialogue()
    {
        Time.timeScale = 0.0f;
        List<int> questIndexs = QuestManager.instance.GetCurrentMainQuestActionIndex();
        DialogueContent dialogueContent = new();
        foreach (var dialogue in _questDialogues)
        {
            if (dialogue.questIndex[0] == questIndexs[0] && dialogue.questIndex[1] == questIndexs[1])
            {
                dialogueContent = _dialogues[dialogue.dialogueIndex];
                break;
            }
        }
        if(dialogueContent.talk == null)
        {
            dialogueContent = _defaultDialogue;
        }
        DialogueManager.instance.StartDialogue(_name, _hideName, _isNameHide, _pnjSprite, dialogueContent, EndDialogue);
    }

    //call at the end of the dialogue
    private void EndDialogue(List<string> choices, bool isNameHide)
    {
        Time.timeScale = 1.0f;
        _isNameHide = isNameHide;
    }

    //call when the player interract with this and this can interact
    protected override void Interact()
    {
        StartDialogue();
    }

    public string GetName => _name;
}

[System.Serializable]
public struct QuestDialogue
{
    public int[] questIndex;
    public int dialogueIndex;
}