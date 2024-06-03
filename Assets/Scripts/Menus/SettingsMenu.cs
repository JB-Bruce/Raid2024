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

    public void OpenSettings()
    {
        isInSettings = true;
        settingsMenu.SetActive(true);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            transform.GetComponent<Canvas>().sortingOrder = 50;
            transform.GetComponent<Canvas>().worldCamera = Camera.main;

            settingsMenu.SetActive(false);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
        //Keeps the settings and sound managers from being destroyed on scene change
        DontDestroyOnLoad(transform.parent);

        //Links volume sliders to their respective functions
        _mainVolumeSlider.onValueChanged.AddListener(delegate { OnMainVolumeChange(); });
        _musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        _sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });

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
    }

    public void OnMusicVolumeChange()
    {
        SoundManager.instance.MusicVolume(_musicVolumeSlider.value);
    }

    public void OnSFXVolumeChange()
    {
        SoundManager.instance.SFXVolume(_sfxVolumeSlider.value);
    }
}
