using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Background Images: ")]
    [SerializeField] Image _currentSlide;
    [SerializeField] int _currentSlideIndex = 0;
    [SerializeField] List<Sprite> _cinematicSlides = new List<Sprite>();

    [Header("Text: ")]
    [SerializeField] TMP_Text _currentCineText;
    [SerializeField] int _currentTextIndex = 0;
    [SerializeField] List<CinematicText> _cineTexts = new List<CinematicText>();

    private Coroutine _textCoroutine;

    [Header("Typing Text Settings: ")]
    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float _timeBetweenChars = 0.05f;
    [SerializeField] bool _startOnEnable = false;

    private string _leadingChar = "";
    private bool _leadingCharBeforeDelay = false;

    [Header("Scene Management: ")]
    [SerializeField] string _sceneToLoadAtEnd;

    private void Awake()
    {
        if (_currentCineText != null && _cineTexts.Count > 0)
        {
            _currentCineText.text = _cineTexts[_currentTextIndex].text;
        }
    }

    private void Start()
    {
        if (_currentCineText != null)
        {
            _currentCineText.text = "";
        }
    }

    public void StartCinematicOnTrigger() //Starts the Cinematic when Trigger is Triggered
    {
        StartCinematicText();
    }

    private void OnDisable() //Stop all runnign coroutines
    {
        StopAllCoroutines();
    }

    private void StartCinematicText() //Starts the Cinematic coroutine
    {
        if(_textCoroutine != null )
        {
            StopCoroutine(_textCoroutine);
        }

        _currentCineText.text = "";
        _textCoroutine = StartCoroutine("TypeCineText");
    }

    public void NextText() //Next Text changes the current displayed text to the next text of the list as well as the next slide if the Cinematic text is a slide changer
    {
        CinematicText cinematicText = _cineTexts[_currentTextIndex];

        if (cinematicText.isEndSlide) //If the Cinematic Text is a Cinematic Ender, loads Scene by name
        {
            SceneManager.LoadScene(_sceneToLoadAtEnd);
        }
        if (cinematicText.isSlideChanger)
        {
            _currentSlideIndex = (_currentSlideIndex + 1) % _cinematicSlides.Count;
            _currentSlide.sprite = _cinematicSlides[_currentSlideIndex];
        }

        _currentTextIndex = (_currentTextIndex + 1) % _cineTexts.Count;
        StartCinematicText();
    }

    IEnumerator TypeCineText() //Types the text at a defined pace
    {
        _currentCineText.text = _leadingCharBeforeDelay ? _leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        string writer = _cineTexts[_currentTextIndex].text;

        foreach (char c in writer)
        {
            if (_currentCineText.text.Length > 0)
            {
                _currentCineText.text =_currentCineText.text.Substring(0, _currentCineText.text.Length - _leadingChar.Length);
            }

            _currentCineText.text += c;
            _currentCineText.text += _leadingChar;
            yield return new WaitForSeconds(_timeBetweenChars);
        }

        if (_leadingChar != "")
        {
            _currentCineText.text = _currentCineText.text.Substring(0, _currentCineText.text.Length - _leadingChar.Length);
        }
    }

}
