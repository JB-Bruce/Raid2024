using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterCustomisation : MonoBehaviour
{
    public static CharacterCustomisation Instance;

    public Image CharacterPreview;
    public Image CharacterPreviewHip;

    public Sprite WomanSprite;
    public Sprite WomanSpriteHip;

    public Sprite ManSprite;
    public Sprite ManSpriteHip;


    public SpriteRenderer PlayerBody;
    public SpriteRenderer PlayerHip;


    public Image BackgroundTuto;
    public Sprite BackgroundTutoIntellectuel;
    public Sprite BackgroundTutoMilitaire;
    public Sprite BackgroundTutoUtopiste;
    public Sprite BackGroundTutoSurvivaliste;

    public Image WomanSymbol;
    public Image ManSymbol;

    public Image FactionSelected;

    public Sprite ManSymbolIntellectuel;
    public Sprite ManSymbolMilitaire;
    public Sprite ManSymbolUtopiste;
    public Sprite ManSymbolSurvivaliste;

    public Sprite WomanSymbolIntellectuel;
    public Sprite WomanSymbolMilitaire;
    public Sprite WomanSymbolUtopiste;
    public Sprite WomanSymbolSurvivaliste;
    
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
        CharacterPreviewHip.sprite = WomanSpriteHip;

        _characterGender = "Woman";
    }

    /// <summary>
    ///     Set a variable for the gender of the character
    /// </summary>
    public void SetCharacterToMan()
    {
        CharacterPreview.sprite = ManSprite;
        CharacterPreviewHip.sprite = ManSpriteHip;
        _characterGender = "Man";
    }

    /// <summary>
    ///     Set a variable to set the character faction to survivalist
    /// </summary>
    public void SetFactionToSurvivalist()
    {
        BackgroundTuto.sprite = BackGroundTutoSurvivaliste;
        ManSymbol.sprite = ManSymbolSurvivaliste;
        WomanSymbol.sprite = WomanSymbolSurvivaliste;

        FactionSelected.enabled = true;
        FactionSelected.rectTransform.anchoredPosition = new Vector2(0, -625);
        _characterFaction = "Survivalist";
    }

    /// <summary>
    /// Set a variable to set the character faction to utopist
    /// </summary>
    public void SetFactionToUtopist()
    {
        BackgroundTuto.sprite = BackgroundTutoUtopiste;
        ManSymbol.sprite = ManSymbolUtopiste;
        WomanSymbol.sprite = WomanSymbolUtopiste;

        FactionSelected.enabled = true;
        FactionSelected.rectTransform.anchoredPosition = new Vector2(0, -0);
        _characterFaction = "Utopist";
    }
    /// <summary>
    ///     Set a variable to set the character faction to Scientist
    /// </summary>
    public void SetFactionToScientist()
    {
        BackgroundTuto.sprite = BackgroundTutoIntellectuel;
        ManSymbol.sprite = ManSymbolIntellectuel;
        WomanSymbol.sprite = WomanSymbolIntellectuel;

        FactionSelected.enabled = true;
        FactionSelected.rectTransform.anchoredPosition = new Vector2(0, -420);
        _characterFaction = "Scientist";
    }

    /// <summary>
    ///     Set a variable to set the character faction to Military
    /// </summary>
    public void SetFactionToMilitary()
    {
        BackgroundTuto.sprite = BackgroundTutoMilitaire;
        ManSymbol.sprite = ManSymbolMilitaire;
        WomanSymbol.sprite = WomanSymbolMilitaire;

        FactionSelected.enabled = true;
        FactionSelected.rectTransform.anchoredPosition = new Vector2(0, -200);
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
            PlayerPrefs.SetString("ChooseFaction", _characterFaction);


            PlayerBody.sprite = CharacterPreview.sprite;
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
