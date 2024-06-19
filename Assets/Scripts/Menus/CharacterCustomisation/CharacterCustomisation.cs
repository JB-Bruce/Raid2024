using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class CharacterCustomisation : MonoBehaviour
{
    public static CharacterCustomisation Instance;

    public Image CharacterPreview;
    public Image CharacterPreviewHair;
    public Image CharacterPreviewHip;

    public Sprite WomanSprite;
    public Sprite WomanSpriteHair;
    public Sprite WomanSpriteHip;

    public Sprite ManSprite;
    public Sprite ManSpriteHair;
    public Sprite ManSpriteHip;


    public SpriteRenderer PlayerHair;
    public SpriteRenderer PlayerBody;
    public SpriteRenderer PlayerHip;
    
    /*
    public Sprite PlayerSpriteBody;
    public Sprite PlayerSpriteHair;
    public Sprite PlayerSpriteHip;*/


    public Toggle ToggleSkipTutorial;

    public bool SkipTutorial;
    public bool PlayPressed = false;

    public TMP_InputField CharacterName;

    public GameObject CharacterSelectionUI;

    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private List<GameObject> _gameobjectToActivate = new List<GameObject>();

    private string _characterNameText = "";
    private string _characterFaction = "";
    private string _characterGender = "";

    /// <summary>
    ///  set an instance for the CharacterCustomisation
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    /// <summary>
    ///     Set a variable for the gender of the character
    /// </summary>
    public void SetCharacterToWoman()
    {
        CharacterPreview.sprite = WomanSprite;
        CharacterPreviewHair.rectTransform.sizeDelta = new Vector2(139, 157);
        CharacterPreviewHair.rectTransform.anchoredPosition = new Vector2(0, 81);
        CharacterPreviewHair.sprite = WomanSpriteHair;
        CharacterPreviewHip.sprite = WomanSpriteHip;

        _characterGender = "Woman";
    }

    /// <summary>
    ///     Set a variable for the gender of the character
    /// </summary>
    public void SetCharacterToMan()
    {
        CharacterPreview.sprite = ManSprite;
        CharacterPreviewHair.rectTransform.sizeDelta = new Vector2(134, 112);
        CharacterPreviewHair.rectTransform.anchoredPosition = new Vector2(0, 100);
        CharacterPreviewHair.sprite = ManSpriteHair;
        CharacterPreviewHip.sprite = ManSpriteHip;
        _characterGender = "Man";
    }

    /// <summary>
    ///     Set a variable to set the character faction to survivalist
    /// </summary>
    public void SetFactionToSurvivalist()
    {

        _characterFaction = "Survivalist";
    }

    /// <summary>
    /// Set a variable to set the character faction to utopist
    /// </summary>
    public void SetFactionToUtopist()
    {
        _characterFaction = "Utopist";
    }
    /// <summary>
    ///     Set a variable to set the character faction to Scientist
    /// </summary>
    public void SetFactionToScientist()
    {
        _characterFaction = "Scientist";
    }

    /// <summary>
    ///     Set a variable to set the character faction to Military
    /// </summary>
    public void SetFactionToMilitary()
    {
        _characterFaction = "Military";
    }

    /// <summary>
    ///     Set a variable to skip the tutorial from a toggle
    /// </summary>
    public void SetSkipTutorial()
    {
        SkipTutorial = ToggleSkipTutorial.isOn;
    }

    /// <summary>
    ///     Set a variable for the character name from a text input
    /// </summary>
    public void SetCharacterName()
    {
        _characterNameText = CharacterName.text;
    }

    /// <summary>
    ///     Called when button play is pressed, Save the character cutsomization and Deactivate the UI
    /// </summary>
    public void OnPlayPressed()
    {
        if(_characterGender != "" && _characterNameText != "" && _characterFaction != "")
        {
            PlayerPrefs.SetString("Gender", _characterGender);
            PlayerPrefs.SetString("CharacterFaction", _characterFaction);
            PlayerPrefs.SetString("CharacterName", _characterNameText);


            PlayerBody.sprite = CharacterPreview.sprite;
            PlayerHair.sprite = CharacterPreviewHair.sprite;
            PlayerHip.sprite = CharacterPreviewHip.sprite;

            PlayPressed = true;
            _playerInput.SwitchCurrentActionMap("InGame");
            ActiveGameUI();
        }
    }


    /// <summary>
    ///     Active the Game UI et deactive the Character selection UI
    /// </summary>
    private void ActiveGameUI()
    {
        foreach(GameObject go in _gameobjectToActivate) 
        {
            go.SetActive(true);
        }
        
        CharacterSelectionUI.SetActive(false);
    }
}
