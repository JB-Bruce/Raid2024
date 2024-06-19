using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Background Images: ")]
    [SerializeField] SpriteRenderer _currentSlide;
    [SerializeField] int _currentSlideIndex = 0;
    [SerializeField] List<Sprite> _cinematicSlides = new List<Sprite>();

    [Header("Text: ")]
    [SerializeField] TMP_Text _currentCineText;
    [SerializeField] int _currentTextIndex = 0;
    [SerializeField] List<CinematicText> _cineTexts = new List<CinematicText>();

    private Coroutine _autoNextCoroutine;

    [Header("Scene Management: ")]
    [SerializeField] string _sceneToLoadAtEnd;

    [Header("Camera")]
    [SerializeField] Camera _camera;
    [SerializeField] Animator _animator;

    private Animator _textAnimator;
    private SoundManager _soundManager;

    private void Awake()
    {
        if (_currentCineText != null && _cineTexts.Count > 0)
        {
            _currentCineText.text = _cineTexts[_currentTextIndex].text;
        }

        _textAnimator = _currentCineText.GetComponent<Animator>();
    }

    private void Start()
    {
        _soundManager = SoundManager.instance;

        if (_currentCineText != null)
        {
            _currentCineText.text = "";
        }
    }

    public void StartCinematicOnTrigger() //Starts the Cinematic when Trigger is Triggered
    {
        _soundManager.PlayMusicFromPlaylist("Cinematic");
        _animator.Play("Slide1", 0, 0);
        StartCinematicSequence();
    }

    private void OnDisable() //Stop all running coroutines
    {
        StopAllCoroutines();
    }

    private void StartCinematicSequence() //Starts the Cinematic coroutine
    {
        if (_autoNextCoroutine != null)
        {
            StopCoroutine(_autoNextCoroutine);
        }

        _currentCineText.text = _cineTexts[_currentTextIndex].text;

        _textAnimator.Play("FadeIn", 0, 0f);

        _autoNextCoroutine = StartCoroutine(AutoNextText());
    }

    public void NextText() //Next Text changes the current displayed text to the next text of the list as well as the next slide if the Cinematic text is a slide changer
    {
        CinematicText cinematicText = _cineTexts[_currentTextIndex];

        if (cinematicText.isEndSlide) //If the Cinematic Text is a Cinematic Ender, loads Scene by name
        {
            _soundManager.PlayMusicFromPlaylist("InGame");
            SceneManager.LoadScene(_sceneToLoadAtEnd);
        }
        if (cinematicText.isSlideChanger)
        {
            _currentSlideIndex = (_currentSlideIndex + 1) % _cinematicSlides.Count;
            _currentSlide.sprite = _cinematicSlides[_currentSlideIndex];
            _animator.Play("Slide" + (_currentSlideIndex + 1), 0, 0);
        }

        _currentTextIndex = (_currentTextIndex + 1) % _cineTexts.Count;
        StartCinematicSequence();
    }

    IEnumerator AutoNextText() //Starts the Coroutine to automatically change to the next Text and Slide
    {
        CinematicText cinematicText = _cineTexts[_currentTextIndex];

        yield return new WaitForSeconds(cinematicText.displayDuration);

        // Play FadeOut animation
        _textAnimator.Play("FadeOut", 0, 0f);

        // Wait for FadeOut animation to complete
        AnimatorStateInfo stateInfo = _textAnimator.GetCurrentAnimatorStateInfo(0);
        float fadeOutDuration = stateInfo.length;
        yield return new WaitForSeconds(fadeOutDuration);

        // Proceed to next text
        NextText();
    }
}
