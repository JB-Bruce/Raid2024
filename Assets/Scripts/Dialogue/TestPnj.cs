using System.Collections.Generic;
using UnityEngine;

public class TestPnj : MonoBehaviour
{
    [SerializeField]
    private List<DialogueContent> dialogues;

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

    //call the DialogueManager to start the dialogue
    public void StartDialogue()
    {
        _startButton.SetActive(false);
        DialogueManager.instance.StartDialogue(_name, _hideName, _isNameHide, _pnjSprite, dialogues[0], EndDialogue);
    }

    //call at the end of the dialogue
    private void EndDialogue(List<string> choices, bool isNameHide)
    {
        _startButton.SetActive(true);
        _isNameHide = isNameHide;
        for (int i = 0; i < choices.Count; i++)
        {
            print(choices[i] + "\n");
        }
    }
}
