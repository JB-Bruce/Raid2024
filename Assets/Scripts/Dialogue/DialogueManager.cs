using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _pnjNameText;
    [SerializeField]
    private TextMeshProUGUI _dialogueText;

    [SerializeField]
    private GameObject _pnjImage;

    [SerializeField]
    private int _dialogueTextMaxLines;

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
    private List<GameObject> _choicesUiList = new();

    private DialogueContent _currentTalk;

    private Action<List<string>, bool> _returnMethode;

    private List<string> _displayedDialogue = new();
    private List<string> _dialogueChoices = new();
    [SerializeField]
    private string _playerName;
    private string _pnjName;
    private string _pnjHideName;

    private bool _isPnjNameHide;

    public static DialogueManager Instance;

    //create an instance of the DialogueManager
    private void Awake()
    {
        Instance = this;
    }

    //create a list of the choices gameobject references at the creation of the script
    private void Start()
    {
        _choicesUiList.Add(_firstChoice);
        _choicesUiList.Add(_secondChoice);
    }

    //called by pnj to start a dialogue
    public void StartDialogue(string pnjName, string pnjHideName, bool isPnjNameHide, Sprite pnjSprite, DialogueContent talk, Action<List<string>, bool> returnMethode)
    {

        _displayedDialogue.Clear();
        _dialogueChoices.Clear();
        _currentTalk = talk;
        _returnMethode = returnMethode;
        _pnjName = pnjName;
        _pnjHideName = pnjHideName;
        _isPnjNameHide = isPnjNameHide;
        _dialogueBox.SetActive(true);
        _dialogueText.SetText(string.Empty);
        if(isPnjNameHide)
        {
            UpdatePnjUi(pnjName, pnjSprite);
        }
        else
        {
            UpdatePnjUi(pnjHideName, pnjSprite);
        }
        UpdateDialogueUi();
        UpdateChoicesButtons();
    }

    //update the visual of the dialogue
    private void UpdateDialogueUi()
    {
        if (_currentTalk.showRealName)
        {
            _isPnjNameHide = false;
            _pnjNameText.SetText(_pnjHideName);
        }

        string speakerName;
        
        if (_currentTalk.isThePlayer)
        {
            speakerName = _playerName;
        }
        else
        {
            if (_isPnjNameHide)
            {
                speakerName = _pnjName;
            }
            else
            {
                speakerName = _pnjHideName;
            }
        }
        print(_currentTalk.talk);
        UpdateDialogue(speakerName, _currentTalk.talk);
    }

    //update the dialogue in the code
    private void UpdateDialogue(string speakerName, string talk)
    {
        _displayedDialogue.Add("<color=blue>" + speakerName + "</color> : " + talk + "\n\n");
        FillDialogueText();

        if (_dialogueText.textInfo.lineCount > _dialogueTextMaxLines)
        {
            _displayedDialogue.RemoveAt(0);
            FillDialogueText();
        }
    }

    //fill the dialogue text
    private void FillDialogueText() 
    {
        _dialogueText.SetText(string.Empty);
        for (int i = 0; i < _displayedDialogue.Count; i++)
        {
            _dialogueText.SetText(_dialogueText.text + _displayedDialogue[i]);
        }
        _dialogueText.ForceMeshUpdate();
    }

    //update the visual pnj interface in the dialoguebox
    private void UpdatePnjUi(string pnjName, Sprite pnjSprite)
    {

        _pnjImage.GetComponent<Image>().sprite = pnjSprite;
        _pnjNameText.SetText(pnjName);
    }

    //update the choices buttons and the skip button
    private void UpdateChoicesButtons()
    {
        if(_currentTalk.choices.Count > 0)
        {
            EnableChoicesButtons();
        }
        else
        {
            DesableChoicesButtons();
        }
    }

    //activate the choices visuals and deactivate skip button
    private void EnableChoicesButtons()
    {
        _choices.SetActive(true);
        _skipButton.SetActive(false);
        for (int i = 0; i < _currentTalk.choices.Count; i++)
        {
            _choicesUiList[i].SetActive(true);
            _choicesUiList[i].GetComponentInChildren<TextMeshProUGUI>().SetText(_currentTalk.choices[i].choice);
        }
    }

    //deactivate the choices buttons and activate skip button
    private void DesableChoicesButtons()
    {
        _choices.SetActive(true);
        _skipButton.SetActive(true);
        for (int i = 0; i < _choicesUiList.Count; i++)
        {
            _choicesUiList[i].SetActive(false);
        }
    }

    //update the visual and the script to display the next dialogue
    public void NextTalk(int talkNumber)
    {
        _currentTalk = _currentTalk.nextTalk[talkNumber];
        UpdateDialogueUi();
        UpdateChoicesButtons();
    }

    //called when a choice button is pressed to go to the next dialogue or end the dialogue
    public void Choice(int talkNumber)
    {
        _dialogueChoices.Add(_currentTalk.choices[talkNumber].choice);
        UpdateDialogue(_playerName, _currentTalk.choices[talkNumber].choice);
        if (_currentTalk.nextTalk.Count > 0)
        {
            NextTalk(_currentTalk.choices[talkNumber].indexNextDialogue);
        }
        else
        {
            _dialogueBox.SetActive(false);
            _returnMethode(_dialogueChoices, _isPnjNameHide);
        }
    }
}


[System.Serializable]
public struct DialogueContent
{
    public bool isThePlayer;
    public string talk;
    public List<DialogueContent> nextTalk;
    public List<Choice> choices;
    public bool showRealName;
}

[System.Serializable]
public struct Choice
{
    public string choice;
    public int indexNextDialogue;
}