using System.Collections.Generic;
using UnityEngine;

public class Pnj : Interactable
{
    [SerializeField]
    private List<DialogueContent> _dialogues = new();

    [SerializeField]
    private List<QuestDialogue> _questDialogues = new();

    [SerializeField]
    private DialogueContent _defaultDialogue = new();

    [SerializeField]
    private Sprite _pnjSprite;

    [SerializeField]
    private string _name;

    [SerializeField]
    private string _hideName;

    private bool _isNameHide;

    [SerializeField]
    private GameObject _startButton;

    public void Start()
    {
        _isNameHide = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInteraction.Instance.interactables.Remove(this);
        }
    }

    //call the DialogueManager to start the dialogue
    public void StartDialogue()
    {
        if (_startButton != null)
        {
            _startButton.SetActive(false);
        }
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
        if (_startButton != null)
        {
            _startButton.SetActive(true);
        }
        _isNameHide = isNameHide;
    }
}

[System.Serializable]
public struct QuestDialogue
{
    public int[] questIndex;
    public int dialogueIndex;
}