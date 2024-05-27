using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("References: ")]
    [SerializeField] AudioSource _musicPlayer;
    [SerializeField] AudioSource _sfxPlayer;

    [Header("Clips: ")]
    [SerializeField] Sounds[] _musics, _sfxs;

    SettingsMenu _settingsMenu;

    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;
    private float _mainVolume = 1f; // Global volume multiplier

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _settingsMenu = SettingsMenu.instance;
        PlayMusic("Theme");
    }

    public void PlayMusic(string _name)
    {
        Sounds _s = Array.Find(_musics, _x =>  _x.name == _name);
        if (_s == null) //If doesn't find music by name
        {
            Debug.Log("Music not Found");
        }
        else //else play the music by name
        {
            _musicPlayer.clip = _s.clip;
            _musicPlayer.Play();
        }
    }

    public void PlaySFX(string _name)
    {
        Sounds _s = Array.Find(_sfxs, _x => _x.name == _name);
        if (_s == null) //If doesn't find sfx by name
        {
            Debug.Log("SFX not Found");
        }
        else //else play the sfx by name
        {
            _sfxPlayer.PlayOneShot(_s.clip);
        }
    }

    public void ToggleMusic() //Allows muting Music
    {
        _musicPlayer.mute = !_musicPlayer.mute; 
    }

    public void ToggleSFX() //Allows muting sfx
    {
        _sfxPlayer.mute = !_sfxPlayer.mute;
    }

    public void MusicVolume(float volume) //Changes the Music's volume
    {
        _musicVolume = volume;
        UpdateVolumes();
    }

    public void SFXVolume(float volume) //Changes the SFX's volume
    {
        _sfxVolume = volume;
        UpdateVolumes();
    }

    public void MainVolume (float volume) //Changes the Main volume
    {
        _mainVolume = volume;
        UpdateVolumes();
    }

    private void UpdateVolumes() //Updates the volume changes
    {
        _musicPlayer.volume = _musicVolume * _mainVolume;
        _sfxPlayer.volume = _sfxVolume * _mainVolume;
    }
}
