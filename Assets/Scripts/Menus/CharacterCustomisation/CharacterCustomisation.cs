using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomisation : MonoBehaviour
{
    public Image CharacterPreview;

    public Sprite WomanSprite;
    public Sprite ManSprite;

    public Toggle ToggleSkipTutorial;

    public bool SkipTutorial;

    public TMP_InputField CharacterName;

    public GameObject CharacterSelectionUI;

    private string CharacterNameText;

    /// <summary>
    ///     Set a variable for the gender of the character
    /// </summary>
    public void SetCharacterToWoman()
    {
        CharacterPreview.sprite = WomanSprite;
        PlayerPrefs.SetString("Gender", "Woman");
    }

    /// <summary>
    ///     Set a variable for the gender of the character
    /// </summary>
    public void SetCharacterToMan()
    {
        CharacterPreview.sprite = ManSprite;

        PlayerPrefs.SetString("Gender", "Man");
    }

    /// <summary>
    ///     Set a variable to set the character faction to survivalist
    /// </summary>
    public void SetFactionToSurvivalist()
    {
        PlayerPrefs.SetString("CharacterFaction", "Survivalist");
    }

    /// <summary>
    /// Set a variable to set the character faction to utopist
    /// </summary>
    public void SetFactionToUtopist()
    {
        PlayerPrefs.SetString("CharacterFaction", "Utopist");
    }
    /// <summary>
    ///     Set a variable to set the character faction to Scientist
    /// </summary>
    public void SetFactionToScientist()
    {
        PlayerPrefs.SetString("CharacterFaction", "Scientist");
    }

    /// <summary>
    ///     Set a variable to set the character faction to Military
    /// </summary>
    public void SetFactionToMilitary()
    {

        PlayerPrefs.SetString("CharacterFaction", "Military");
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
        CharacterNameText = CharacterName.text;
    }

    /// <summary>
    ///     Called when button play is pressed 
    ///     Save character name
    ///     Deactivate the UI
    /// </summary>
    public void OnPlayPressed()
    {
        PlayerPrefs.SetString("CharacterName", CharacterNameText);
        CharacterSelectionUI.SetActive(false);
    }
}
