using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _pnjNameText;
    [SerializeField]
    private TextMeshPro _dialogueText;

    [SerializeField]
    private Image _pnjImage;

    [SerializeField]
    private GameObject _dialogueBox;
    [SerializeField]
    private GameObject _choices;
    [SerializeField]
    private GameObject _firstChoice;
    [SerializeField]
    private GameObject _secondChoice;
    [SerializeField]
    private GameObject _skipButton;

    private List<GameObject> _choicesList;

    private DialogueContent _currentDialogue;
    private Action<string> _returnMethode;

    //create a list of the choices gameobject references at the creation of the script
    private void Start()
    {
        _choicesList.Add(_firstChoice);
        _choicesList.Add(_secondChoice);
    }

    //methode called by pnj to start a dialogue
    public void StratDialogue(DialogueContent dialogue, Action<string> returnMethode)
    {
        _currentDialogue = dialogue;
        _returnMethode = returnMethode;
        UpdatePnjUi();
        UpdateDialogueUi();
        _dialogueBox.SetActive(true);
    }

    //methode updated the visual of the dialogue
    private void UpdateDialogueUi()
    {
        _dialogueText.SetText(_dialogueText.text + "\n\n<color=blue>"+ _currentDialogue.pnjName + "</color> : " + _currentDialogue.talk);
    }

    //methode updated the visual pnj interface in the dialoguebox
    private void UpdatePnjUi()
    {
        _pnjImage.sprite = _currentDialogue.pnjImage;
        _pnjNameText.SetText(_currentDialogue.pnjName);
    }

    //activate the choices visuals
    private void EnableChoicesUi(DialogueContent dialogue) 
    {
        for (int i = 0; i < dialogue.choices.Count; i++)
        {
            _choicesList[i].SetActive(true);
            _choicesList[i].GetComponent<TextMeshProUGUI>().SetText(dialogue.choices[i]);
        }
        _choices.SetActive(true);
    }

    //methode call when a choice button is pressed to return the choice
    public void Choice(int number)
    {
        _dialogueBox.SetActive(false);
        _returnMethode(_currentDialogue.choices[number]);
    }
}


[System.Serializable]
public struct DialogueContent
{
    public string pnjName;
    public string talk;
    public Sprite pnjImage;
    public List<DialogueContent> nextDialogue;
    public List<string> choices;
}
