using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu instance;

    [Header("Sliders: ")]
    [SerializeField] Slider _mainVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;

    [Header("References: ")]
    public GameObject settingsMenu;

    [Header("Booleans: ")]
    public bool isInSettings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            transform.parent.GetComponent<Canvas>().sortingOrder = 50;
            transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;

            gameObject.SetActive(false);
        }
        else
        {
            Destroy(transform.parent.parent.gameObject);
        }
        //Keeps the settings and sound managers from being destroyed on scene change
        DontDestroyOnLoad(transform.parent.parent);

        //Links volume sliders to their respective functions
        _mainVolumeSlider.onValueChanged.AddListener(delegate { OnMainVolumeChange(); });
        _musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        _sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });

    }

    private void Start()
    {
        _mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume", 1);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        _sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public void DeactivateSettingsMenu()
    {
        settingsMenu.SetActive(false);
        isInSettings = false;
    }
    
    public void ToggleMusic()
    {
        SoundManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        SoundManager.instance.ToggleSFX();
    }

    public void OnMainVolumeChange()
    {
        SoundManager.instance.MainVolume(_mainVolumeSlider.value);
        PlayerPrefs.SetFloat("mainVolume", _mainVolumeSlider.value);
    }

    public void OnMusicVolumeChange()
    {
        SoundManager.instance.MusicVolume(_musicVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolume", _musicVolumeSlider.value);
    }

    public void OnSFXVolumeChange()
    {
        SoundManager.instance.SFXVolume(_sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", _sfxVolumeSlider.value);
    }
}
